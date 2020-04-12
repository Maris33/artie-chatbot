using System;
using System.Collections.Generic;
using System.Text;

namespace ShowArtistPictures
{
    public class Artists
    {
        public List<ArtistsInfo> artistInfo { get; set; }

        public Artists()
        {
            this.artistInfo = new List<ArtistsInfo>();
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Hieronymus Bosch", "Bosch" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Ilya Repin", "Repin" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "El Greco", "Greco" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Leonardo da Vinci", "da Vinci" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Pierre-Auguste Renoir", "Renoir" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Claude Monet", "Monet" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Michelangelo Buonarroti", "Buonarrot" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Raphael Santi", "Raphael" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Rene Magritte", "Magritte" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Edward Hopper", "Hopper" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Frida Kahlo", "Kahlo", "Frida" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Ivan Aivazovsky", "Aivazovsky" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Thomas Eakins", "Eakins" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Eugene Delacroix", "Delacroix" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Vincent van Gogh", "van Gogh" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Piet Mondrian", "Mondrian" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Peter Paul Rubens", "Rubens" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Wassily Kandinsky", "Kandinsky" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Michelangelo Merisi da Caravaggio", "Caravaggio" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Sandro Botticelli", "Botticelli" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Tiziano Vecelli", "Tiziano" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Max Ernst", "Ernst" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Francisco Goya", "Goya" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Kazimir Malevich", "Malevich" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Rembrandt", "Rembrandt" }
                )
            );
            this.artistInfo.Add(
                new ArtistsInfo(
                       new List<string> { "Salvador Dali", "Dali" }
                )
            );
        }

        public bool isDefined(String stringToCheck)
        {
            foreach (ArtistsInfo value in artistInfo)
            {
                if (value.isDefined(stringToCheck))
                {
                    return true;
                }
            }
            return false;
        }

        public ArtistsInfo getArtistByName(String artistName)
        {
            foreach (ArtistsInfo value in artistInfo)
            {
                if (value.isDefined(artistName))
                {
                    return value;
                }
            }
            return null;
        }

        public bool HasRequiredArtistInfoFields
        {
            get
            {
                return artistInfo.Count > 0;
            }
        }
        public class ArtistsInfo
        {
            public List<String> _values { get; set; }

            public ArtistsInfo(List<String> values)
            {
                _values = values;
            }

            public ArtistsInfo()
            {
                _values = new List<string>();
            }
            public void addValue(String value)
            {
                _values.Add(value);
            }

            public bool isDefined(String stringToCheck)
            {
                foreach (String value in _values)
                {
                    if (value.Equals(stringToCheck))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
