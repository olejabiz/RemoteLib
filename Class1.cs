using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace RemoteLib
{
    public class BookStore : MarshalByRefObject
    {
        public void startApp()
        {
            Console.WriteLine("Приложение запущено!");
            ILease lease = (ILease)GetLifetimeService();
            MyClientSponsor sponsor = new MyClientSponsor();
            lease.Register(sponsor);
        }

        public override Object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromSeconds(3);
                lease.SponsorshipTimeout = TimeSpan.FromSeconds(10);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }


    }

    public class MyClientSponsor : MarshalByRefObject, ISponsor
    {
        private DateTime lastRenewal;
        int count = 0;

        public MyClientSponsor()
        {
            Console.WriteLine("Спонсор создан ");
            lastRenewal = DateTime.Now;
        }
        public TimeSpan Renewal(ILease lease)
        {
            count++;
            Console.WriteLine("Вызван метод Renewal спонсора {0}-ый раз", count);
            Console.WriteLine("Время с момента последнего вызова:" + (DateTime.Now - lastRenewal).ToString());
            lastRenewal = DateTime.Now;
            return TimeSpan.FromSeconds(10);
        }
    }
}
