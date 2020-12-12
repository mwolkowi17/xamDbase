using HelloWorld;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UsingBase
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public class Recipe
        {
            [PrimaryKey,AutoIncrement]
            public int Id { get; set; }
            [MaxLength(255)]
            public string Name { get; set; }
        }

        public SQLiteAsyncConnection _connection;
        public ObservableCollection<Recipe> _recipes;

        public MainPage()
        {
            InitializeComponent();
           _connection =  DependencyService.Get<ISQLiteDb>().GetConnection();
           

        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Recipe>();
            var recipes = await _connection.Table<Recipe>().ToListAsync();
            var _recipes = new ObservableCollection<Recipe>(recipes);
            recipesListView.ItemsSource = _recipes;
            base.OnAppearing();
        }

         async void OnAdd(object sender, System.EventArgs e)
        {
            var recipe = new Recipe { Name = "Recipe" + DateTime.Now.Ticks };
            await _connection.InsertAsync(recipe);
            //_recipes.Add(recipe);
            var recipes = await _connection.Table<Recipe>().ToListAsync();
            var _recipes = new ObservableCollection<Recipe>(recipes);
            recipesListView.ItemsSource = _recipes;

        }

        async void OnUpdate(object sender, System.EventArgs e)
        {
            var recipes = await _connection.Table<Recipe>().ToListAsync();
            var _recipes = new ObservableCollection<Recipe>(recipes);
            var recipe = _recipes[0];
            recipe.Name += "Updated";
            await _connection.UpdateAsync(recipe);
            recipes = await _connection.Table<Recipe>().ToListAsync();
            _recipes = new ObservableCollection<Recipe>(recipes);
            recipesListView.ItemsSource = _recipes;
        }

        async void OnDelete(object sender, System.EventArgs e)
        {
            var recipes = await _connection.Table<Recipe>().ToListAsync();
            var _recipes = new ObservableCollection<Recipe>(recipes);
            var recipe = _recipes[0];
            await _connection.DeleteAsync(recipe);
            recipes = await _connection.Table<Recipe>().ToListAsync();
            _recipes = new ObservableCollection<Recipe>(recipes);
            //_recipes.Remove(recipe);
            recipesListView.ItemsSource = _recipes;
        }
    }
}
