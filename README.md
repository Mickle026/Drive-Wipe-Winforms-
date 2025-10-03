# Drive Wipe

![App Icon](icon.ico)

Drive Wipe is a lightweight **C# WinForms utility** for secure file and drive wiping.  
It provides a simple drag-and-drop interface to permanently erase files and folders, making them unrecoverable.

---

## ✨ Features
- 🖱️ **Drag & Drop** support for quick file wiping
- 🔒 Secure overwrite methods
- 📂 Wipe entire folders or individual files
- 🪟 Simple WinForms GUI
- 🖼️ Custom icons and visuals

---

## 📂 Project Structure
```
DriveWipe/
├── App.config
├── Drive Wipe.csproj
├── Program.cs
├── Form1.cs / Form1.Designer.cs / Form1.resx
├── DropTargetForm.cs / DropTargetForm.Designer.cs / DropTargetForm.resx
├── Resources/
│   └── Shred.jpg
└── Icons/
    ├── icon.ico
    └── icon for disk wipe a.png
```

---

## 🚀 Getting Started

### Prerequisites
- Windows 10/11
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (Community Edition works)
- .NET Framework 4.8

### Build Instructions
1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/DriveWipe.git
   ```
2. Open `Drive Wipe.csproj` in Visual Studio.
3. Restore NuGet packages (if any).
4. Press **F5** to build and run.

---

## 📖 Usage
1. Run the application.
2. Drag and drop files/folders into the main window.
3. Confirm the wipe operation.
4. Files will be securely erased and cannot be recovered.

⚠️ **Warning:** Wiped data is permanently destroyed. Use carefully!

---

## 🛠️ Technologies
- C# (.NET Framework 4.8)
- WinForms (Windows Forms)
- Custom UI with icons and resources

---

## 📸 Screenshots

> Placeholder for screenshots. Add your application UI images here, for example:

![Screenshot Placeholder](https://via.placeholder.com/600x400?text=Drive+Wipe+Screenshot)

---

## 📜 License
This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.
