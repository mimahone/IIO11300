/*
* Copyright (C) JAMK/IT/Esa Salmikangas
* This file is part of the IIO11300 course project.
* Created: 15.1.2016 Modified: 15.1.2016
* Authors: Mika Mähönen (K6058) ,Esa Salmikangas
*/

namespace Tehtava1
{
  public class BusinessLogicWindow
  {
    /// <summary>
    /// CalculateWindowArea calculates the area of a window
    /// </summary>
    public static double CalculateWindowArea(double width, double height, double frameWidth)
    {
      if (width <= 0 || height <= 0) return 0;
      return (width - (2 * frameWidth)) * (height - (2 * frameWidth));
    }

    /// <summary>
    /// CalculatePerimeter calculates the perimeter of a window
    /// </summary>
    public static double CalculatePerimeter(double width, double height)
    {
      if (width <= 0 || height <= 0) return 0;
      return width * 2 + height * 2;
    }

    /// <summary>
    /// CalculateFrameArea calculates the area of a frame
    /// </summary>
    public static double CalculateFrameArea(double width, double height, double frameWidth)
    {
      if (width <= 0 || height <= 0) return 0;
      return (width * frameWidth * 2) + ((height - (2 * frameWidth)) * frameWidth * 2);
    }
  }
}
