# Sidebar Scroll Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** 为主页面左侧导航栏提供更丝滑的滚动体验和更现代化的极简滚动条样式。

**Architecture:** 通过在导航 `ListBox` 上增加专用 class，将滚动条主题覆盖限制在左侧导航区域内。样式逻辑集中放在应用级 `App.axaml`，主窗口仅负责挂载 class 和滚动相关附加属性，保持结构清晰且复用成本低。

**Tech Stack:** Avalonia 11、FluentTheme、XAML 样式选择器、模板内选择器

---

### Task 1: 挂载导航专用样式入口

**Files:**
- Modify: `hospital_ward/Views/MainWindow.axaml`

**Step 1: 为导航 ListBox 添加专用 class**

- 在左侧栏 `ListBox` 上增加 `sidebar-nav` class。

**Step 2: 保持现有绑定与滚动配置不变**

- 不改动 `ItemsSource`、`SelectedItem`、`ItemTemplate`。

### Task 2: 在应用级样式中定义导航滚动主题

**Files:**
- Modify: `hospital_ward/App.axaml`

**Step 1: 添加导航专属 ScrollViewer 配置**

- 设置 `AllowAutoHide`、`IsScrollInertiaEnabled` 等属性。

**Step 2: 覆写纵向 ScrollBar 外观**

- 设置窄边距、透明背景、显示/隐藏延迟。

**Step 3: 覆写 Track 与 Thumb 的默认态**

- 默认 thumb 低透明、细窄、圆角胶囊。

**Step 4: 覆写悬停态与 expanded 态**

- expanded 态增强可见性与厚度感。

**Step 5: 隐藏上下箭头按钮**

- 减少传统滚动条的重量感。

### Task 3: 验证构建结果

**Files:**
- Test: `hospital_ward/App.axaml`
- Test: `hospital_ward/Views/MainWindow.axaml`

**Step 1: 运行构建**

Run: `dotnet build "d:/github/rjgc_2026/hospital_ward/MyFirstApp.csproj"`

Expected: 构建成功，无新增 XAML 编译错误。

**Step 2: 运行程序观察导航栏**

- 确认滚动条默认态更轻，悬停与展开过渡更顺滑。
