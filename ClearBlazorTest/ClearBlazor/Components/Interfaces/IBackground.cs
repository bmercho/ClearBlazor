﻿namespace ClearBlazor
{   
    /// <summary>
    /// Defines the backround color for a component
    /// </summary>

    public interface IBackground
    {
        /// <summary>
        /// Background color of component
        /// </summary>
        public Color? BackgroundColor { get; set; }
    }
}
