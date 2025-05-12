![logo](assets/logo.png)

## 🚀 FloodPrevisionAI

🌊 FloodPrevisionAI is a cutting-edge research and demonstration tool that simulates flood scenarios in a virtual 3D environment and leverages AI to forecast flood events before they happen. By combining realistic water dynamics with an intelligent neural network, the project allows users to explore how changes in terrain, weather inputs, and sensor configurations impact flood risk. Whether you’re a hydrologist, urban planner, educator, or AI enthusiast, FloodPrevisionAI offers an interactive playground to:

* 🌍 **Understand Flood Dynamics**: Watch how virtual water interacts with complex terrain—flowing through valleys, accumulating in basins, and responding to changes in elevation and obstacles.
* 📡 **Test Sensor Strategies**: Experiment with placement and density of virtual sensors measuring water level, flow velocity, and ground elevation. See firsthand how sensor quantity and quality affect prediction accuracy.
* 🤖 **Train and Evaluate AI Models**: Dive into a lightweight C# neural network that processes real-time sensor streams to predict flood onset, severity, and location. Assess model performance under diverse simulated conditions.
* 🎨 **Visualize Results in Real Time**: Color-coded alerts and dynamic graphs update live in Unity’s UI, offering immediate feedback on flood risk (🟢 safe, 🟡 caution, 🔴 alert).
* 📈 **Analyze Historical Runs**: Export detailed logs for post-simulation analysis in Mathematica or Python. Plot time-series trends, compare scenarios side by side, and refine AI hyperparameters.

### 🔑 Key Benefits

* 🛡️ **Safe Experimentation**: Explore extreme flood events without real-world risks.
* 🚀 **Rapid Prototyping**: Modify terrain, sensor logic, or AI parameters and see effects instantly.
* 🎓 **Educational Value**: Use as a teaching aid for hydrology, environmental science, or applied machine learning.
* 🔧 **Open & Extensible**: Built on Unity and C#, it’s easy to extend with new sensors, models, or visualization features.

## 🧩 How to Get Started

1. **Clone** the repo:

```bash
git clone https://github.com/FabioFant/FloodPrevisionAI.git
```
2. **Open** the project in Unity (2020.3 LTS or later).
3. **Play** the `Assets/Scenes/FloodSimulation` scene and watch the magic! ✨

## 📚 Project Structure

```
FloodPrevisionAI/
├── Assets/
│   ├── Scenes/      # Main flood sim scene
│   ├── Scripts/     # Sensor logic, AI model, UI code
│   └── Shaders/     # Water & terrain shaders
├── Analysis/        # Notebooks for post-run data dives
└── README.md        # You are here!
```
