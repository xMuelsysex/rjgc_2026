# MyFirstApp

这是一个基于 [Avalonia UI](https://avaloniaui.net/) 框架和 .NET 8 构建的跨平台桌面应用程序（包含病房床位管理等功能）。

## 功能特性 (Features)

本项目基于 CSDN 博客文章的业务原型进行了全栈开源复刻，完全基于内存数据源，并实现了以下功能管理模块（全模块已完整支持增加、编辑、删除、搜索过滤与实时表格展示功能）：

### 核心管理模块

- 📊 **系统首页 (Dashboard)**
  - 自动归集并提炼关键运营数据：总计床位数、今日空闲/占用/维修中床位数、出入院总人次。
  - 通过九张高亮统计卡片，实时呈现医院整体的运转状态与全院床位占用率。

- 🏢 **科室管理 (Department Management)**
  - 规范设立并管理全院所有医疗科室，包括科室名称缩写、所主治病种范围的描述，以及相应负责人登记。

- 🏥 **病房管理 (Ward Management)**
  - 维护医院所有住院病区资源字典，定义并划分普通病房、高级 VIP 房或是 ICU 重症监护室的区别属性。
  - 为每一个病房设置额定可容纳床位基准数与绑定唯一的所属科室。

- 🛏️ **病房床位管理 (Bed Management)**
  - 管理细粒度到底层物理床位的唯一编码及所属病房关系。
  - 动态监控每一张床位的最新状态（空闲或占用），并且与住院患者形成绑定。

### 医护患档案模块

- 👨‍⚕️ **医生管理 (Doctor Management)**
  - 统一维护医院执业医生档案，追踪所属科室、职称情况(主治/副主任/主任医师)与联系方式。
- 👩‍⚕️ **护士管理 (Nurse Management)**
  - 管理在编护士基础资料与值班科室归属，确保人员合理调度分配。
- 🤕 **患者管理 (Patient Management)**
  - 集中构建结构化的电子病历人口档案池。记录每一位患者精确的基础资料(如性别、身份证、联系方式)并与其当前收治的病房床位与科室发生映射关系。

### 就医全流程追踪模块

- 📝 **入院登记 (Admission Registration)**
  - 高效处置医疗收治入院流程；患者分配专属主治医师，指定床位，并记录详细初步诊断原因与挂号日期。
- 🚪 **出院登记 (Discharge Registration)**
  - 办理出院结算证明手续，总结住院期间所有服务产生的医疗累计总费用。
- ⚕️ **诊疗信息 (Medical Records)**
  - 对抗医疗纠纷或随访追踪：逐日记录历次重大主诉诊断、临床建议与制定的专科治疗方案。
- 💊 **用药信息 (Medication Records)**
  - 对症下药开具全生命周期住院处方清单；完整记录包含给定的通用名称、每次摄取剂量以及使用服用频次要求。

## 环境要求

在运行此项目之前，请确保您的开发环境中已经安装了以下组件：

- [.NET 8.0 SDK](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0) 或更高版本
- 推荐 IDE：Visual Studio 2022、JetBrains Rider，或安装了 C#/Avalonia 扩展的 Visual Studio Code。

---

## 如何运行项目？

### 方法一：使用命令行 (Terminal / CLI) 运行 (推荐)

1. 打开您的终端（如 PowerShell、命令提示符 或 VS Code 内置终端）。
2. 确保您当前所在的目录是项目的根目录（即包含 `MyFirstApp.csproj` 文件的目录）：
   ```bash
   cd d:\github\ruanjiangngcheng_2026\MyFirstApp
   ```
3. 输入以下命令并回车，即可编译并运行程序：
   ```bash
   dotnet run
   ```

### 方法二：使用 Visual Studio 或 Rider 运行

1. 用 IDE 打开 `MyFirstApp.csproj` 文件（或您项目所在的 `.sln` 解决方案文件）。
2. 将 `MyFirstApp` 设为启动项目（通常默认已经是）。
3. 点击顶部的 **"运行" (Start)** 按钮，或直接按键盘上的 `F5` 键。

### 方法三：使用 Visual Studio Code 运行

1. 在 VS Code 中打开 `MyFirstApp` 文件夹。
2. 打开左侧的 "运行和调试" (Run and Debug) 面板（快捷键：`Ctrl+Shift+D`）。
3. 如果系统提示生成 C# 调试配置资源，请点击“Yes”。
4. 点击面板上方的绿色三角形按钮（"Start Debugging"），或按 `F5`。

---

## 常用开发命令备忘录

- **构建项目**（检查是否有编译错误，但不运行）：
  ```bash
  dotnet build
  ```
- **清理项目**（删除旧的编译缓存，有时遇到奇怪错误可以尝试此操作）：
  ```bash
  dotnet clean
  ```
- **发布项目**（为了给其他人打包不带源码的运行程序）：
  ```bash
  dotnet publish -c Release
  ```

---

## 常见问题排查 (Troubleshooting)

在本项目开发和运行过程中，可能会遇到以下两个常见问题。这里记录了原因及解决办法：

### 1. IDE 提示: `当前上下文中不存在名称“InitializeComponent”`

这是在使用 Avalonia (以及 WPF/MAUI) 时非常**常见的 IDE 缓存/分析器假报错**。

- **原因**：`InitializeComponent()` 是由 Avalonia XAML 编译器在构建时自动生成的部分类方法。当你在 `.axaml` 文件中新增内容后，IDE 的后台代码分析器偶尔会跟不上，没有及时读取到生成的代码。
- **验证**：只要在终端中运行 `dotnet build` 能够提示“已成功”，说明代码没有任何实际问题。
- **解决办法**：
  - 在 Visual Studio Code 中：按 `Ctrl+Shift+P` 打开命令面板，输入 `Developer: Reload Window`（重新加载窗口）即可消除红线。
  - 直接**编译**项目（`dotnet build` 或点击 IDE 的生成按钮），多数情况下 IDE 会在编译后自动刷新缓存。

### 2. 页面完全空白（包含表格的页面）

如果你发现系统首页正常，但是点击病房管理、患者管理等页面时，内容是一片空白，这通常是由于 `DataGrid` 控件没有被正确加载引起的。

- **原因**：在 Avalonia 11 版本中，`DataGrid` 被从核心库中移出，成为了一个独立的包。如果没有安装该包或者没有引入该包的主题样式，`DataGrid` 会在界面上静默失败（完全不可见），导致包含它的整个页面无法渲染。
- **解决办法**（本项目已修复此问题）：
  1. 确保安装了 NuGet 包：`Avalonia.Controls.DataGrid`。
  2. 必须在项目的全局文件 `App.axaml` 中引入该控件树的专属官方样式：
     ```xml
     <Application.Styles>
         <FluentTheme />
         <StyleInclude Source="avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"/>
     </Application.Styles>
     ```
