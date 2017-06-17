using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
			Assert.That(() => list.TrueForAll(user.CanSee), Is.True.After((int)timeSpan.TotalMilliseconds, 10),
						$"User can't see all: {string.Join(", ", texts)}");
		}

		public void ShouldNotSee(params string[] texts)
		{
			var list = new List<string>(texts);
			if (!list.Any(user.CanSee))
				return; // NOTE: prevent Assert from waiting 10 ms each time if text is seen immediately
			Assert.That(() => list.TrueForAll(text => !user.CanSee(text)), Is.True.After((int)timeSpan.TotalMilliseconds, 10),
						$"User can see any: {string.Join(", ", texts)}");
		}

		public void Tap(params string[] texts)
		{
			ShouldSee(texts[0]);
			foreach (var text in texts)
				user.Tap(text);
		}
	}
}
