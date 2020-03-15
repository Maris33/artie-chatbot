using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using static KindOfArt.kindOfArt;

namespace KindOfArt
{
    public class KindOfArtInfoIntentProcessor : AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "KindOfArt";
        KindsOfART _chosenArtType = KindsOfART.Null;
        public override LexResponse Process(LexEvent lexEvent, ILambdaContext context)
        {
            IDictionary<string, string> slots = lexEvent.CurrentIntent.Slots;
            IDictionary<string, string> sessionAttributes = lexEvent.SessionAttributes ?? new Dictionary<string, string>();
            //if the KindOfArt slot has a value, validate that it is contained within the enum list available.
            if (slots[TYPE_SLOT] != null)
            {
                var validateKindOfArtType = ValidateKindOfArtType(slots[TYPE_SLOT]);


            }
            return ArtTypeChosen(sessionAttributes, "Fulfilled",
                        new LexResponse.LexMessage
                        {
                            ContentType = MESSAGE_CONTENT_TYPE,
                            Content =String.Format(getMessageForChosenArtType(_chosenArtType))
                        }
                    );
        }

        private String getMessageForChosenArtType(KindsOfART kindOfART)
        {
            switch(kindOfART)
            {
                case KindsOfART.Impressionism:
                    return "learn more about Impressionism";   
                case KindsOfART.Modernism:
                    return "learn more about Modernism";  
                case KindsOfART.Realism:
                    return "learn more about Realism";  
                default:
                    return "I am so sorry :( We don't know about this art type yet. Could you please try again?";
            }
        }

        private ValidationResult ValidateKindOfArtType(string kindOfArtTypeString)
        {
            bool kindOfArtTypeValid = Enum.IsDefined(typeof(KindsOfART), kindOfArtTypeString.ToUpper());

            if (Enum.TryParse(typeof(KindsOfART), kindOfArtTypeString, true, out object kindOfArtType))
            {
                _chosenArtType = (KindsOfART)kindOfArtType;
                return ValidationResult.VALID_RESULT;
            }
            else
            {
                return new ValidationResult(false, TYPE_SLOT, String.Format("We do not have information about {0}, would you like a different kind of ART? Our most popular request is about modernism", kindOfArtTypeString));
            }
        }
    }
}
