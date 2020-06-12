﻿using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;

namespace KindOfArt
{
    /// <summary>
    /// Represents an intent processor that the Lambda function will invoke to process the event.
    /// </summary>
    public interface IIntentProcessor
    {
        /// <summary>
        /// Main method for processing the Lex event for the intent.
        /// </summary>
        /// <param name="lexEvent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        LexResponse Process(LexEvent lexEvent, ILambdaContext context);
    }
}
