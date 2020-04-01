using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MoviesHD.Model
{
    public class AnimatedItemListViewBehaviours : Behavior<SfListView>

    {

        private SfListView listView;

        protected override void OnAttachedTo(BindableObject bindable)

        {

            listView = bindable as SfListView;

            listView.ItemGenerator = new ItemGeneratorExt(listView);

            base.OnAttachedTo(bindable);

        }

    }




public class ItemGeneratorExt : ItemGenerator

    {

        public SfListView ListView { get; set; }

        public ItemGeneratorExt(SfListView listview) : base(listview)

        {

            ListView = listview;

        }

        protected override ListViewItem OnCreateListViewItem(int itemIndex, ItemType type, object data = null)

        {

            if (type == ItemType.Record)

                return new ListViewItemExt(ListView);

            return base.OnCreateListViewItem(itemIndex, type, data);

        }

    }




public class ListViewItemExt : ListViewItem

    {

        private SfListView _listView;



        public ListViewItemExt(SfListView listView)

        {

            _listView = listView;

        }
         
        protected override void OnItemAppearing()
        {            
            this.Opacity = 0;
            TranslationY = -15;
            this.FadeTo(1, 400, Easing.SinInOut);

            
            this.TranslateTo(0, 0,500,easing:Easing.SpringOut);

            base.OnItemAppearing();

        }

    }
}
