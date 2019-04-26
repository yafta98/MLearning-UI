﻿using System;

namespace MLearning_UI.Network_Elements
{
    public class NetworkSize
    {
        public int InputLayerLength { get; }
        public int OutputLayerLength { get; }
        public int[] InternalLayerLengths { get; }
        public int InternalLayerCount { get; }
        public int TotalLayerCount
        {
            get
            {
                return InternalLayerCount + 2;
            }
        }

        public NetworkSize(int inputLayerLength, int[] internalLayerLengths, int outputLayerLength)
        {
            this.InputLayerLength = inputLayerLength;
            this.InternalLayerLengths = internalLayerLengths;
            this.OutputLayerLength = outputLayerLength;
            this.InternalLayerCount = internalLayerLengths.Length;
        }
    }
}