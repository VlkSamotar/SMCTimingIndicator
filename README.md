# SMC Timing Indicator

A custom cTrader indicator that draws vertical lines at user‑defined times to mark key trading sessions or events.  
Designed for Smart Money Concepts (SMC) / ICT strategies, this tool helps visualize intraday timing and market context.

---

## ✨ Features
- Up to **8 configurable vertical lines** per day
- Customizable **time, color, style, and thickness** for each line
- Option to **show historical lines** across past trading days
- **UTC offset parameter** for flexible timezone alignment
- Clean, modular architecture with enums, helpers, and services
- Open‑source, free to use and share

---

## ⚙️ Parameters

| Parameter                | Type        | Default | Description |
|---------------------------|-------------|---------|-------------|
| Line X Time (HH:mm)       | `string`    | varies  | Time of the vertical line (e.g. `08:00`). |
| Line X Color              | `enum`      | varies  | Line color (`White`, `Red`, `Blue`, `Yellow`, `Green`, `Purple`, `Grey`, `Orange`). |
| Line X Style              | `enum`      | Dashed  | Line style (`Solid`, `Dotted`, `Dashed`). |
| Line X Thickness          | `enum`      | One     | Line thickness (1–8). |
| Show Historical Lines?    | `bool`      | false   | If true, draws lines for all past trading days. |
| UTC Offset (minutes)      | `int`       | 60      | Offset from UTC in minutes (e.g. `60` for CET). |

> **Note:** Each line (1–8) has its own set of parameters.  
> Example: `Line 1 Time`, `Line 1 Color`, `Line 1 Style`, `Line 1 Thickness`.

---

## 📖 Usage
1. Install the indicator in **cTrader Automate** (Visual Studio or cTrader Editor).
2. Configure line times and styles in the indicator parameters.
3. Adjust the **UTC offset** to match your local timezone or broker server time.
4. Enable **Show Historical Lines** if you want to see past sessions marked on the chart.

---

## 🛠️ Architecture
- **Enums**: Define line style, color, thickness.
- **Helpers**: `LineVisualMapper` maps enums to cTrader API types.
- **Models**: `LineDefinition` encapsulates line properties and parsing logic.
- **Services**: `LineDrawer` handles drawing logic and UTC conversion.
- **Indicator**: `SMCTimingIndicator` orchestrates initialization, daily updates, and history rendering.

---

## 📷 Screenshots
*(Add chart screenshots here to demonstrate session lines in action.)*

---

## 📦 Installation
- Clone or download this repository.
- Open in **Visual Studio** with cTrader Automate SDK installed.
- Build the project and load the `.algo` file into cTrader.
- Alternatively, import directly via **cTrader Store** (free).

---

## 🔄 Versioning
- **v1.0.0** – Initial release with 8 configurable lines, history option, and UTC offset.

---

## 📜 License
Released under the **MIT License**.  
You are free to use, modify, and distribute this indicator.

---

## 🤝 Contributing
Contributions are welcome!  
- Fork the repo
- Create a feature branch
- Submit a pull request

---

## 📬 Support
For questions or feature requests, open an **Issue** on GitHub.  
Community discussions and feedback are encouraged.