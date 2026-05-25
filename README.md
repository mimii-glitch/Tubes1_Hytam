# 🤖 Tubes 1 IF2211 — Robocode Tank Royale

Repositori ini berisi implementasi bot tank untuk **Tubes Besar 1 IF2211 Strategi Algoritma** menggunakan platform **Robocode Tank Royale v0.30.0**. Semua bot ditulis dalam bahasa **C# (.NET 10.0)**.

---

## 📋 Daftar Bot

| Folder | Nama Bot | Author(s) | Deskripsi |
|---|---|---|---|
| `BlackMan/` | Irengin ajah | hytam | A bot that drives forward and backward, and fires a bullet |
| `botak2/` | Gagah Perkasa | hytam | A bot that drives forward and backward, and fires a bullet |
| `HytamBOT2/` | [Hytam]BOT2 | Jimmy Simatupang, Paulus Siahaan, Lennon Lumbantoruan | Tank Bot dengan implementasi Algoritma Greedy (Dinamis & Anti-Linear) |
| `Bot2/` | Bot 2 | Jimmmi | Sok Keras kau bos |

---

## ⚙️ Prasyarat (Prerequisites)

Pastikan semua software berikut sudah terinstal sebelum menjalankan project ini:

| Software | Versi Minimum | Link Download |
|---|---|---|
| **Java JDK** | 11 atau lebih baru | https://adoptium.net/ |
| **.NET SDK** | 10.0 atau lebih baru | https://dotnet.microsoft.com/download |

Verifikasi instalasi dengan membuka terminal dan jalankan:

```bash
java --version
dotnet --version
```

---

## 🚀 Cara Menjalankan Project

### Langkah 1 — Clone Repository

```bash
git clone <url-repository-ini>
cd tubes1-if2211-starter-pack
```

> **Penting:** Setelah clone, buka file `config.properties` dan ubah path `bot-directories` sesuai lokasi folder project di device kamu (lihat [Konfigurasi](#-konfigurasi)).

---

### Langkah 2 — Jalankan GUI Tank Royale

GUI Tank Royale sudah tersedia sebagai file `.jar` di root folder. Jalankan dengan:

```bash
java -jar robocode-tankroyale-gui-0.30.0.jar
```

> Alternatifnya, kamu juga bisa build sendiri dari source (lihat [Build dari Source](#-build-gui-dari-source-opsional)).

---

### Langkah 3 — Jalankan Bot

Buka **terminal baru** (jangan tutup GUI), lalu masuk ke folder bot yang ingin dijalankan dan eksekusi file `.cmd` (Windows) atau `.sh` (Linux/Mac):

**Windows:**
```bash
cd HytamBOT2
HytamBOT2.cmd
```

**Linux / macOS:**
```bash
cd HytamBOT2
chmod +x HytamBOT2.sh
./HytamBOT2.sh
```

Ulangi langkah ini di terminal terpisah untuk setiap bot yang ingin ikut bertanding.

> Bot akan otomatis melakukan `dotnet build` jika belum pernah di-build sebelumnya, lalu terkoneksi ke server.

---

### Langkah 4 — Mulai Battle di GUI

1. Di jendela GUI Tank Royale, klik **Battle → Start Battle**
2. Pilih bot-bot yang sudah terkoneksi dari daftar
3. Klik **Start** — pertandingan dimulai! 🎮

---

## 🔧 Konfigurasi

### `config.properties`

File ini berisi konfigurasi path folder bot dan pengaturan game. **Wajib diubah** sesuai lokasi project di device kamu:

```properties
# Ganti dengan path absolut ke folder root project ini di device kamu
# Contoh Windows:
bot-directories=C:\\Users\\namauser\\tubes1-if2211-starter-pack,true

# Contoh Linux/macOS:
# bot-directories=/home/namauser/tubes1-if2211-starter-pack,true

game-type=classic
tps=30
```

### `server.properties`

Berisi secret key untuk koneksi antara bot dan server. File ini sudah dikonfigurasi dan tidak perlu diubah.

---

## 🔨 Build GUI dari Source (Opsional)

Jika ingin build ulang GUI Tank Royale dari source code:

```bash
# Masuk ke direktori engine
cd tank-royale-0.30.0

# Build GUI App
./gradlew :gui-app:clean
./gradlew :gui-app:build
```

File `.jar` hasil build akan berada di:
```
tank-royale-0.30.0/gui-app/build/libs/robocode-tankroyale-gui-0.30.0.jar
```

Jalankan dengan:
```bash
java -jar tank-royale-0.30.0/gui-app/build/libs/robocode-tankroyale-gui-0.30.0.jar
```

---

## 🗂️ Struktur Folder

```
tubes1-if2211-starter-pack/
│
├── robocode-tankroyale-gui-0.30.0.jar   ← GUI siap pakai
├── config.properties                     ← Konfigurasi path & game
├── server.properties                     ← Secret key server
│
├── BlackMan/                             ← Bot: Irengin ajah (C#)
│   ├── BlackMan.cs
│   ├── BlackMan.json
│   ├── BlackMan.cmd
│   └── BlackMan.sh
│
├── botak2/                               ← Bot: Gagah Perkasa (C#)
│   ├── botak2.cs
│   ├── botak2.json
│   ├── botak2.cmd
│   └── botak2.sh
│
├── HytamBOT2/                            ← Bot: [Hytam]BOT2 (C#)
│   ├── HytamBOT2.cs
│   ├── HytamBOT2.json
│   ├── HytamBOT2.cmd
│   └── HytamBOT2.sh
│
├── Bot2/                                 ← Bot: Bot 2 (C#)
│   ├── Bot2.cs
│   ├── Bot2.json
│   ├── Bot2.cmd
│   └── Bot2.sh
│
└── tank-royale-0.30.0/                   ← Source engine Tank Royale
```

---

## 🐛 Troubleshooting

### Bot tidak terdeteksi di GUI
- Pastikan `config.properties` sudah diubah ke path project yang benar di device kamu.
- Pastikan bot sudah dijalankan (`*.cmd` / `*.sh`) **setelah** GUI dibuka.
- Cek apakah `.NET SDK 10.0` sudah terinstal: `dotnet --version`

### Error `dotnet build` gagal
- Pastikan versi .NET SDK minimal **10.0**.
- Coba hapus folder `bin/` dan `obj/` di dalam folder bot, lalu jalankan ulang `*.cmd`.

### GUI tidak bisa dibuka
- Pastikan Java JDK terinstal: `java --version`
- Coba jalankan dengan versi Java yang sesuai (minimal JDK 11).

---

## 📚 Referensi

- [Robocode Tank Royale Documentation](https://robocode-dev.github.io/tank-royale/)
- [Tank Royale Bot API for .NET](https://robocode-dev.github.io/tank-royale/api/dotnet/)
- [Robocode Tank Royale GitHub](https://github.com/robocode-dev/tank-royale)

---

*IF2211 Strategi Algoritma — Institut Teknologi Bandung*
