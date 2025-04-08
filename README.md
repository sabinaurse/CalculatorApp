# CalculatorApp

**CalculatorApp** is a C# WPF application that replicates core functionalities of the Windows Calculator, supporting both Standard and Programmer modes. It provides basic and scientific operations, memory management, digit grouping, and number system conversions, all in a fixed-size, console-style window.

## Features

- Basic arithmetic operations (+, -, *, /, %, √, x², 1/x, +/-).
- Memory functions (M+, M-, MS, MR, Memory Stack).
- Standard and Programmer modes (binary, octal, decimal, hexadecimal conversions).
- Cut, Copy, Paste implemented manually via strings.
- Digit grouping based on system locale (using CultureInfo).
- Real-time result updates during input (cascading calculations).
- Persistent user settings across sessions (mode, number base, digit grouping).
- Keyboard and mouse input support.
- Fixed-size window; non-editable display.
- Custom Help menu with About section.

## Technologies Used

- C# (.NET 8)
- WPF (Windows Presentation Foundation)
