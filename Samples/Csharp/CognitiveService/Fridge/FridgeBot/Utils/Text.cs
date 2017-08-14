using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FridgeBot.Utils
{
    public static class Text
    {
        public const string Greet = "Hi, I am your fridge, Fridge Bot :) \n\nWhat can I do for you today? Say \'help\' for help.";
        public const string NotYetSupported = "Sorry, we do not support that action yet.";
        public const string NoItemToAdd = "Did you forget to mention what you want to add? Try again.";
        public const string NoItemToRemove = "Did you forget to mention what you want to remove? Try again.";
        public const string Added = "Thank you for the {0} Appreciate it :)";
        public const string RemovedSucc = "Here\'s the {0}! It\'s no longer in my tummy.";
        public const string RemovedErr = "I don\'t have {0} in my stomach; are you sure you fed it to me?";
        public const string Show = "I have {0} in my stomach!";
        public const string Help = "I am your Fridge Bot, and I take care of your fridge inventory. "
            + "You can put things in, take things out and see what is in the fridge. "
            + "You can say \'put apple\' and I will add an apple, "
            + "or \'remove apple\' and I will remove an apple. "
            + "You can say \'show what you have\' and I will show you the inventory, "
            + "or say \'clear all\' and I will clear out the fridge for you. "
            + "Try it out now and say \'add an apple\' :). "
            + "This sample bot does not support numerics.";
        public const string Clear = "Are you sure you want to clear up the fridge? Say yes or no.";
        public const string ClearYes = "I cleared up myself! I am hungry now because I am empty.";
        public const string ClearNo = "Whew. I knew it! I will keep everything.";
        public const string Yes = "yes";
    }
}