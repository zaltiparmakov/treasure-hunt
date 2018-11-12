using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using System.IO;

namespace LovNaZaklad_WebAPI.EmguCV
{
    public class ImageComparer
    {
        public float[] features(Stream file)
        {
            Bitmap bitmap = (Bitmap)Image.FromStream(file);
            Image<Gray, Byte> img = new Image<Gray, Byte>(bitmap);

            calculate_distance_LBP(ref img, 3);
            float[] features = HOG(ref img);

            return features;
        }
        
        private void calculate_distance_LBP(ref Image<Gray, Byte> modImg, int distance)
        {
            // It's essentially just basic LBP, but the checking of the neighbouring value is based on user input, but you're also calculating the center value instead of using it as a threshold.
            int[] data = new int[8];
            int buffIndex = distance;
            int[] buff = new int[8];
            for (int i = 1; i < modImg.Rows - 1; i++)
            {
                for (int j = 1; j < modImg.Cols - 1; j++)
                {
                    //var center = modImg.Data[i, j, 0];
                    buff[0] = modImg.Data[i - 1, j, 0];
                    buff[1] = modImg.Data[i - 1, j + 1, 0];
                    buff[2] = modImg.Data[i, j + 1, 0];
                    buff[3] = modImg.Data[i + 1, j + 1, 0];
                    buff[4] = modImg.Data[i + 1, j, 0];
                    buff[5] = modImg.Data[i + 1, j - 1, 0];
                    buff[6] = modImg.Data[i, j - 1, 0];
                    buff[7] = modImg.Data[i - 1, j - 1, 0];
                    // Base Position               // + Distance Position
                    data[0] = (modImg.Data[i - 1, j, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[1] = (modImg.Data[i - 1, j + 1, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[2] = (modImg.Data[i, j + 1, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[3] = (modImg.Data[i + 1, j + 1, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[4] = (modImg.Data[i + 1, j, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[5] = (modImg.Data[i + 1, j - 1, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[6] = (modImg.Data[i, j - 1, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex += distance;
                    if (buffIndex > 7)
                    {
                        buffIndex -= 7;
                        buffIndex -= 1;
                    }
                    data[7] = (modImg.Data[i - 1, j - 1, 0] > buff[buffIndex]) ? 1 : 0;
                    buffIndex = 0;

                    int val = Convert.ToInt32(string.Join("", data), 2);
                    modImg.Data[i, j, 0] = (byte)val;
                }
            }
        }

        public float[] HOG(ref Image<Gray, Byte> modImg)
        {
            // Square-Root Normalization - compresses the input pixel less than Gamma. Increases accuracy of HOG
            //CvInvoke.Sqrt(newMatrix, newMatrix);
            // make the image 64x128 - recommended for HOG description
            int h = modImg.Height;
            double scaleBy = h / 256.0;
            int width = (int)((double)modImg.Width / scaleBy);
            int height = (int)((double)modImg.Height / scaleBy);
            modImg = modImg.Resize(width, height, Emgu.CV.CvEnum.Inter.Linear);

            /* Compute the Gradient Vector of every pixel, as well as magnitude and direction */
            // apply Sobel by x and y
            Image<Gray, float> sobel = modImg.Sobel(0, 1, 3).Add(modImg.Sobel(1, 0, 3)).AbsDiff(new Gray(0.0));
            modImg = sobel.Convert<Gray, Byte>();

            /* Compute descriptor values */
            HOGDescriptor hog = new HOGDescriptor();
            Size WinStride = new Size(18, 18);
            Size Padding = new Size(10, 10);
            Point[] locations = null;
            float[] descriptorValues = hog.Compute(modImg, WinStride, Padding, locations);

            return descriptorValues;
        }

        private double compareResults(float[] img1, float[] img2)
        {
            return 1;
        }
    }
}