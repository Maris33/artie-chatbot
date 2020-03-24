using System;
using System.Collections.Generic;
using System.Text;

namespace KindOfArt
{

    /// A class to store all the current values from the intent's slots. 
    /// Intend - kindOfArtInfo
    /// Slot - KindsOfART

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
