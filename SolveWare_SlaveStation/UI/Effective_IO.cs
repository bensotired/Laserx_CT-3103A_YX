using SolveWare_IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_SlaveStation
{
    //单个轴绑定UI

    public class Effective_IOs : ArrayObject<Effective_IO>
    {
        public Effective_IOs()
        {

        }

        public Effective_IO this[string name]
        {
            get
            {
                return this.collection.Find(a => a.OnOffButton?.Name == name);
            }
        }
    }

    public class Effective_IO//有效的IO
    {
        public Label StatusLabel { get; set; }
        public IOBase iOBase { get; set; }
        public Button OnOffButton { get; set; }   //输入的IO是没有这个功能的

        public void SetStatusLabelName()
        {
            this.StatusLabel.Text = this.iOBase.Name;
        }
    }


    public class ArrayObject<T> where T : class
    {
        public ArrayObject()
        {
            this.collection = new List<T>();
        }
        public List<T> collection { get; set; }

        public void Add(T t)
        {
            this.collection.Add(t);
        }
    }

}
