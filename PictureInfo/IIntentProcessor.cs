using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace PictureInfo
{
    public interface IIntentProcessor
    {
        LexResponse Process(LexEvent lexEvent, ILambdaContext context);
    }
}
