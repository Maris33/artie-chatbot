using System;
using System.Collections.Generic;
using System.Text;

namespace KindOfArtArtists
{
    public class kindOfArt
    {
        public KindsOfART? kindOfART { get; set; }
        public bool HasRequiredKindOfARTFields
        {
            get
            {
                return !string.IsNullOrEmpty(kindOfART.ToString());

            }
        }

        public enum KindsOfART
        {
            Realism,
            Impressionism,
            Modernism,
            Baroque,
            Abstractionism,
            Renaissance,
            Romanticism,
            Surrealism,
            Null
        }
    }
}
