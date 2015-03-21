using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using NUnit.Framework;
using SpirvNet.DotNet;
using SpirvNet.DotNet.CFG;

namespace SpirvNet.Tests
{

    [TestFixture]
    public class ShaderTest
    {
        public struct VSInput
        {
            public vec3 Position;
            public vec2 TexCoord;

            public vec3 ParticlePosition;
            public vec4 ParticleColor;
            public float ParticleSize;
            public float ParticleAngle;
            public float ParticleLife;
        }

        public struct VSOutput
        {
            public vec4 Position;

            public vec3 Normal;
            public vec3 WorldPos;
            public vec4 ParticleColor;
            public vec2 TexCoord;
            public float Life;
        }

        public class ParticleShader
        {
            public mat4 ViewMatrix;
            public mat4 ModelMatrix;
            public mat4 InverseViewMatrix;
            public mat4 ProjectionMatrix;

            public VSOutput VertexShader(VSInput a)
            {
                var v = new VSOutput();

                v.TexCoord = a.TexCoord;

                v.ParticleColor = vec4.Clamp(a.ParticleColor, 0, 1);

                // world space position:
                vec4 worldPos = ModelMatrix * new vec4(a.ParticlePosition, 1.0f);
                v.WorldPos = (vec3)worldPos;
                v.Life = a.ParticleLife;

                vec4 eyePos = ViewMatrix * worldPos;

                vec2 tangent = new vec2(
                            (float)Math.Cos(a.ParticleAngle),
                            (float)Math.Sin(a.ParticleAngle)
                            );
                vec2 normal = new vec2(
                            -tangent.y,
                            tangent.x
                            );

                float st = vec2.Smoothstep(0.0f, 0.7f, a.ParticleLife).x;

                float size = a.ParticleSize * (0.25f + 0.75f * st);

                size *= 1 + 1 * st;

                eyePos += new vec4(tangent * (a.Position.x * size)
                                 + normal * (a.Position.y * size));

                v.Normal = new vec3(InverseViewMatrix * new vec4(0, 0, -1, 0));

                // projected vertex position used for the interpolation
                v.Position = ProjectionMatrix * eyePos;
                return v;
            }

            public vec3 BranchingShader(vec2 v)
            {
                if (v.Length < 0.5)
                {
                    return v.swizzle.grg;
                }
                else
                {
                    for (int i = 0; i < v.LengthSqr; ++i)
                        v += vec2.UnitX * i;

                    switch (v.Count)
                    {
                        case 0:
                            return vec3.UnitX;
                        case 1:
                            return vec3.UnitY;
                        case 2:
                            return vec3.UnitZ;
                        default:
                            v += vec2.Ones;
                            break;
                    }
                }

                return vec3.MaxValue - new vec3(v);
            }
        }

        [Test]
        public void ParticleShaderTest()
        {
            var shader = new ParticleShader();
            var def = CecilLoader.DefinitionFor((Func<VSInput, VSOutput>)shader.VertexShader);
            Assert.AreEqual("VertexShader", def.Name);

            //File.WriteAllLines(@"C:\Temp\shadertestdump.csv", CecilLoader.CsvDump(def));
        }

        [Test]
        public void BranchingShaderTest()
        {
            var shader = new ParticleShader();
            var def = CecilLoader.DefinitionFor((Func<vec2, vec3>)shader.BranchingShader);
            Assert.AreEqual("BranchingShader", def.Name);

            var cfg = new ControlFlowGraph(def);
            Assert.Greater(cfg.Vertices.Count, 30);

            //File.WriteAllLines(@"C:\Temp\shadertestdump-branching.csv", CecilLoader.CsvDump(def));
            //File.WriteAllLines(@"C:\Temp\shadertestdump-branching.dot", cfg.DotFile);
        }
    }
}
