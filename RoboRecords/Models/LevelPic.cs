/*
 * LevelPic.cs
 * Copyright (C) 2021, Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace RoboRecords.Models
{
    public class LevelPic
    {
        // TODO (IMPORTANT): This solution only works under Windows.
        // Needs to use a cross-platform library instead of System.Drawing
        static byte[,] palette = new byte[256, 3];
        // TODO: Look for this in the correct place instead of project root
        const string PALETTE_NAME = "PLAYPAL";


        public Bitmap picture { get; set; }
        public string filename { get; set; }

        public LevelPic(byte[] file, string name)
        {
            ReadPalette(PALETTE_NAME);
            picture = ColoredBitmapFromBytes(file);
            filename = name;
        }


        // Ors' methods to read and convert image lmp to bitmap
        static byte[][] PicToPaletteArray(byte[] fileBytes)
        {
            int currentByte = 0;
            int w = DataReader.ReadUInt16(ref currentByte, fileBytes);
            int h = DataReader.ReadUInt16(ref currentByte, fileBytes);
            int x_off = DataReader.ReadUInt16(ref currentByte, fileBytes);
            int y_off = DataReader.ReadUInt16(ref currentByte, fileBytes);

            const int HEADER_SIZE = 8;
            Int32 phase = HEADER_SIZE;

            byte[][] image = new byte[w][];
            for (int i = 0; i < w; i++)
            {
                image[i] = new byte[h];
                for (int j = 0; j < h; j++)
                {
                    image[i][j] = 255;
                }
            }

            UInt32[] column_array = new UInt32[w];

            for (int i = 0; i < w; i++)
            {
                column_array[i] = DataReader.ReadUInt32(ref phase, fileBytes);
            }

            for (int i = 0; i < w; i++)
            {
                phase = (int)column_array[i];
                int rowstart = 0;
                while (rowstart != 255)
                {
                    rowstart = fileBytes[phase++];
                    if (rowstart == 255)
                    {
                        break;
                    }
                    byte pixel_count = fileBytes[phase++];
                    byte dummy_value = fileBytes[phase++];

                    for (int j = 0; j < pixel_count; j++)
                    {
                        byte pixel = fileBytes[phase++];
                        image[i][j + rowstart] = pixel;
                    }
                    dummy_value = fileBytes[phase++];
                }
            }
            return image;
        }

        private Bitmap ArrayToBitmap(byte[][] image)
        {
            int w = image.Length;
            int h = image[0].Length;
            Bitmap bitmap = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    byte col = image[x][y];
                    if (col == 255)
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                    }
                    else
                    {
                        byte r = palette[col, 0];
                        byte g = palette[col, 1];
                        byte b = palette[col, 2];
                        bitmap.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                    }
                }
            }
            return bitmap;
        }

        private Bitmap ColoredBitmapFromBytes(byte[] pic)
        {
            byte[][] image = PicToPaletteArray(pic);
            return ArrayToBitmap(image);
        }

        static void ReadPalette(string fileName)
        {
            byte[] paletteBytes = File.ReadAllBytes(fileName);
            int color = 0;
            int index = 0;
            foreach (byte b in paletteBytes)
            {
                palette[index, color] = b;
                color++;
                if (color >= 3)
                {
                    color = 0;
                    index++;
                }
            }
        }

        // TODO: This location is likely gonna be wrong in live version
        public static void SaveToFile(Bitmap bitmap, string filename)
        {
            using (Bitmap bmp = bitmap)
            {
                bmp.Save(@"wwwroot\assets\images\mappics\" + filename + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
