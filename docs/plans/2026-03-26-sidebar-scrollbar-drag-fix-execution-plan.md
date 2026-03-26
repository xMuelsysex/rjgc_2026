# Sidebar Scrollbar Drag Fix Execution Plan

**Internal Grade:** M

## Wave 1: Freeze And Inspect

- Read the current `App.axaml` scrollbar styles.
- Compare the current thumb width, margin, and transform against Avalonia Fluent defaults.
- Confirm whether the thumb hit target was reduced below a usable range.

## Wave 2: Minimal Fix

- Remove the aggressive thumb scale transform on vertical and horizontal scrollbars.
- Reduce thumb margins to preserve a slimmer look without collapsing the draggable area.
- Keep the opacity, color, and expanded-state styling that do not interfere with drag behavior.

## Wave 3: Verification

- Run `dotnet build "d:/github/rjgc_2026/hospital_ward/MyFirstApp.csproj"`.
- Relaunch the desktop app.
- Confirm the left navigation scrollbar can be grabbed and dragged normally.

## Rollback Rule

- If the change makes the scrollbar visually unacceptable, revert only the thumb sizing adjustments and keep the rest of the scrollbar theme intact.

## Cleanup Expectation

- Leave requirement and plan artifacts in `docs/`.
- Avoid temp files.
- Record the final verification result in the session response.
