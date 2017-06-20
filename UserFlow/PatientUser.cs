using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.Forms;

namespace UserFlow
{
    public class PatientUser
    {
        readonly TimeSpan timeSpan;
        readonly User user;

        public PatientUser(User user, TimeSpan timeSpan)
        {
            this.user = user;
            this.timeSpan = timeSpan;
        }

        public void ShouldSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (list.All(user.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => list.All(user.CanSee), Is.True.After((int)timeSpan.TotalMilliseconds, 10),
                        $"User can't see all: {string.Join(", ", texts)}");
        }

        public void ShouldNotSee(params string[] texts)
        {
            var list = new List<string>(texts);
            if (!list.Any(user.CanSee))
                return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
            Assert.That(() => !list.Any(user.CanSee), Is.True.After((int)timeSpan.TotalMilliseconds, 10),
                        $"User can see any: {string.Join(", ", texts)}");
        }

        protected Element Find(string text)
        {
            ShouldSee(text);
            return user.Find(text);
        }

        public void Tap(params string[] texts)
        {
            foreach (var text in texts) {
                ShouldSee(text);
                user.Tap(text);
            }
        }
    }
}
