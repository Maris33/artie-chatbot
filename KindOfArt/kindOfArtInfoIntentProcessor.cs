using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using static KindOfArt.kindOfArt;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace KindOfArt
{
    public class KindOfArtInfoIntentProcessor : AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "artKind";
        KindsOfART _chosenArtType = KindsOfART.Null;
        private Task<ScanResponse> result;

        public KindOfArtInfoIntentProcessor(Task<ScanResponse> result)
        {
            this.result = result;
        }

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
                            Content = String.Format(getMessageForChosenArtType(_chosenArtType))
                        }
                    );
        }

        private String getMessageForChosenArtType(KindsOfART kindOfART)
        {
            List<Item> kindOfArtItems = result.Result.Items.Select(Map).ToList();
            String resultDescription = "I am so sorry :( We don't know about this art type yet. Could you please try again?";
            foreach (Item item in kindOfArtItems)
            {
                if (item.KindOfArtName.Equals(kindOfART.ToString()))
                {
                    resultDescription = item.Description;
                    break;
                }
            }

            return resultDescription;
        }

        public class Item
        {
            public string KindOfArtName { get; set; }
            public string Description { get; set; }
        }

        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item
            {
                KindOfArtName = result["KindOfArtName"].S,
                Description = result["Description"].S
            };
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
