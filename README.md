# SpirvNet
SPIR-V generator for .NET IL

## Goals

* Decode, Verify, Encode SPIR-V files
* Convert (suitable) .NET IL to SPIR-V
* Write Shaders and Kernels in C#
* Debug Shaders and Kernels in C# (CPU fallback)
* Use Gpu.For in addition to Parallel.For

## Technologies used

* [Mono.Cecil](http://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/)
* [GlmSharp](https://github.com/Philip-Trettner/GlmSharp)

## About SPIR-V

SPIR-V is a new binary intermediate language for representing graphical-shader stages and compute kernels for multiple Khronos APIs.

Resources:
* [Provisional SPIR-V Specification](https://www.khronos.org/registry/spir-v/specs/1.0/SPIRV.pdf)
* [SPIR-V Go Encoder/Verifier/Decoder](https://github.com/jteeuwen/spirv)
* [GlslangValidator with SPIR-V output](https://www.khronos.org/opengles/sdk/tools/Reference-Compiler/)
* [LunarGLASS SPIR-V Front-End](http://www.lunarglass.org/)

## Current Status

* working OpCode serialization and Module Encode/Decode
* work-in-progress .NET IL analysis
