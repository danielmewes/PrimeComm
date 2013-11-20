﻿using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace PrimeComm
{
    /// <summary>
    /// Class for hpprgm files
    /// </summary>
    class PrimeProgramFile
    {
        /// <summary>
        /// Parses the data inside a file to be used later
        /// </summary>
        /// <param name="path">Input file</param>
        public PrimeProgramFile(string path)
        {
            IsValid = false;
            Name = Path.GetFileNameWithoutExtension(path);
            Data = new byte[0];
            var b = File.ReadAllBytes(path);

            switch (Path.GetExtension(path))
            {
                case ".txt":
                    Data = Encoding.Convert(Encoding.Default, Encoding.Unicode, b);
                    IsValid = true;
                    break;
                    
                default:
                    if (b.Length >= 20)
                    {
                        if (b[0] == 0x0c && b[8] == 0x00) // Unnamed and supported
                        {
                            var size = b[16] + b[17]*0xff + b[18]*0xff*0xff;
                            Data = new byte[size];

                            const int offset = 20;
                            for (int i = offset; i < offset + size && i < b.Length; i++)
                                Data[i - offset] = b[i];

                            IsValid = true;
                        }
                    }
                    break;
            }
        }

        public byte[] Data { get; set; }

        public string Name { get; set; }

        public bool IsValid { get; set; }
    }
}