![nanoFramework logo](https://raw.githubusercontent.com/nanoframework/Home/main/resources/logo/nanoFramework-repo-logo.png)

-----

# Welcome to the .NET **nanoFramework** M5 CoreInk repository

## Firmware

The CoreInk can be flashed through `nanoff` flashing tool with the command line:

```shell
nanoff --target ESP32_PICO --update --serialport COM3
```

## Implementation

The static class `M5CoreInk` provides pre-configured properties for ready-to-go use of the core.

Here's a list of the current implementation

| Component | Property name | Implemented | Tested |
|:-|---|---|---|
| Left wheel button<sup>1</sup> | RollerLeft | :heavy_check_mark: | :heavy_check_mark: |
| Middle wheel button<sup>1</sup> | RollerButton | :heavy_check_mark: | :heavy_check_mark: |
| Right wheel button<sup>1</sup> | RollerRight | :heavy_check_mark: | :heavy_check_mark: |
| Button (top position) | Button | :heavy_check_mark: | :heavy_check_mark: |
| Power button | Power | :heavy_check_mark: | :x: |
| Green led | Led | :heavy_check_mark: | :heavy_check_mark: |
| Buzzer | Buzzer | :heavy_check_mark: | :x: |
| BM8563 | RTC | :heavy_check_mark: | :heavy_check_mark: |
| ADC/DAC pins | GetAdcGpio() | :heavy_check_mark: | :x: |
| Screen | :x: | :x: | :x: |
| EXT-PORT | :x: | :x: | :x: |

## Notes

<sup>1</sup> The implementation define the buttons as `PinMode.Input` as defined in the [specifications](https://m5stack.oss-cn-shenzhen.aliyuncs.com/resource/docs/datasheet/core/esp32_datasheet_en_v3.9.pdf) Table.26

```text
GPIO pins 34-39 are input-only.
These pins do not feature an output driver or internal pull-up/pull-down circuitry.
The pin names are: SENSOR_VP (GPIO36), SENSOR_CAPP (GPIO37), SENSOR_CAPN (GPIO38), SENSOR_VN (GPIO39), VDET_1 (GPIO34), VDET_2 (GPIO35).
```