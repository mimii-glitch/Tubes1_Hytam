using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

namespace Hytam
{
    public class BOT2 : Bot
    {
        // Variabel untuk arah dodge (1 = maju/kanan, -1 = mundur/kiri)
        private double moveDirection = 1;
        private Random rnd = new Random();

        // --- VARIABEL BARU UNTUK 1 VS BANYAK (MELEE) ---
        private int? targetId = null;            // ID musuh yang sedang dikunci
        private double targetDistance = double.MaxValue; // Jarak terakhir target yang dikunci
        private int lastSeenTurn = 0;            // Tick terakhir saat target terlihat

        static void Main(string[] args)
        {
            new BOT2().Start();
        }

        BOT2() : base(BotInfo.FromFile("HytamBOT2.json")) { }

        public override void Run()
        {
            // Kustomisasi warna tank
            BodyColor = Color.DarkSlateGray;
            GunColor = Color.Black;
            RadarColor = Color.Red;
            BulletColor = Color.Orange;

            // Radar Cepat: Putar radar mencari target di awal ronde
            SetTurnRadarRight(Double.PositiveInfinity);

            while (IsRunning)
            {
                // GREEDY RADAR TIMEOUT:
                // Jika target terdekat yang dikunci menghilang/kabur selama lebih dari 3 frame (tick),
                // lepas kuncian secara instan dan putar radar 360 derajat lagi untuk mencari target terdekat baru.
                if (targetId != null && TurnNumber - lastSeenTurn > 3)
                {
                    ResetTarget();
                }

                // Eksekusi semua perintah yang sudah di-set
                Go(); 
            }
        }

        // Fungsi pembantu untuk mereset pencarian radar
        private void ResetTarget()
        {
            targetId = null;
            targetDistance = double.MaxValue;
            SetTurnRadarRight(Double.PositiveInfinity);
        }

        public override void OnScannedBot(ScannedBotEvent e)
        {
            double distance = DistanceTo(e.X, e.Y);
            double enemyBearing = BearingTo(e.X, e.Y);

            // FUNGSI SELEKSI GREEDY: 
            // Ambil keputusan mengunci target JIKA:
            // 1. Belum punya target (targetId == null)
            // 2. Bot yang terscan adalah target lama kita (e.ScannedBotId == targetId)
            // 3. Ada bot baru yang posisinya LEBIH DEKAT daripada target saat ini (distance < targetDistance)
            if (targetId == null || e.ScannedBotId == targetId || distance < targetDistance)
            {
                targetId = e.ScannedBotId;
                targetDistance = distance;
                lastSeenTurn = TurnNumber;
            }
            else
            {
                // Jika ada bot lain yang tertangkap radar tapi jaraknya lebih jauh, 
                // secara GREEDY kita ABAIKAN agar fokus lock tidak terganggu.
                return;
            }

            double absoluteDirection = Direction + enemyBearing;

            // 1. FIRE POWER DINAMIS (Berdasarkan Jarak Target Terdekat)
            double firePower = 1.5; 
            if (distance < 150) {
                firePower = 4.0;
            } else if (distance < 300) {
                firePower = 2.0;
            }

            // 2. GUN HEAT CONTROL & AIMING
            double gunTurn = CalcDeltaAngle(GunDirection, absoluteDirection);
            SetTurnGunRight(gunTurn);

            // Jika sudut aim terkecil (kurang dari 5 derajat) dan gun siap, langsung tembak!
            if (Math.Abs(gunTurn) < 5 && GunHeat == 0)
            {
                SetFire(firePower);
            }

            // 3. MOVEMENT & TEKNIK DODGE (Orbiting Tegak Lurus Terhadap Target Terdekat)
            double turnAngle = enemyBearing + 90 - (15 * moveDirection);
            SetTurnRight(turnAngle);

            if (distance < 300) 
            {
                SetBack(100);
            } 
            else 
            {
                if (rnd.NextDouble() > 0.8) // 20% peluang ganti arah mendadak (Zig-zag)
                {
                    moveDirection *= -1; 
                }
                SetForward(150 * moveDirection);
            }

            // 4. RADAR LOCK PERMANEN (Sempit & Akurat)
            // Kunci radar dengan sapuan kecil (+-10 derajat) hanya pada area target terdekat.
            // Karena kita sudah menyaring data di atas, radar tidak akan slip ke musuh lain yang jauh.
            double radarTurn = CalcDeltaAngle(RadarDirection, absoluteDirection);
            radarTurn += (radarTurn < 0 ? -10 : 10);
            SetTurnRadarRight(radarTurn);
        }

        // JIKA MUSUH YANG SEDANG DIKUNCI MATI (Greedy Cleanup)
        public override void OnBotDeath(BotDeathEvent e)
        {
            // Jika target yang kita buru mati dibunuh bot lain atau oleh kita, langsung reset radar
            if (targetId != null && e.VictimId == targetId)
            {
                ResetTarget();
            }
        }

        public override void OnBulletFired(BulletFiredEvent e)
        {
            moveDirection *= -1;
            SetForward(100 * moveDirection);
        }

        public override void OnHitWall(HitWallEvent e)
        {
            moveDirection *= -1;
            SetForward(150 * moveDirection);
        }


    }
}