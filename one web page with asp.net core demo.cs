--> complete one web page with asp.net core 
/**migration commands -->
*-->add-migration name    
*--> update-database
**/

1) connect project to mssql database


//#create DbConClass in model folder , pass domain model class to dbset to create it's table in the db


namespace ModelDbCon.Models
{
    //here you add the db conn string and db name with the help of override method OnConfiguring
    
    
    //inherit from dbContext class its a refrence represent a db session

    public class DbConClass:DbContext

    { //constructor
        public DbConClass():base()
            { 
        
        }

        //add the dbset which is the domain model class that represent the db tables

        public DbSet<Student> Student { get; set; }
        public DbSet<Department> Department { get; set; }

        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<Course> Course { get; set; }


        //this is to specify which db server you're using and the db name and the auth type
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //pass the connection string, initial catalog is the db name u creating and integrated security is the auth true-> windows auth
            //false-> pass the username and pass if u choose auth in ur db

            optionsBuilder.UseSqlServer("Data Source=Atty;Initial Catalog=ITI_Db;Integrated Security=True");
        }

    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////

 
2) create domain model class(map to db table )

//# create Department.cs class

namespace ModelDbCon.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ManagerName { get; set; }
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////



3) crete controller class to manipulate db and view(action method that map to html page) that display it 


//to see that page on browser (url):controllerName/ActionMethodName :
//action method is the name you put in url to go the page on browser 

/*to get new department data from user you should add 2 methods one to show an empty form so the user could fill it
*and one to save the form into the db
*/

//create DeptController class in controller folder
namespace ModelDbCon.Controllers
{
    public class DeptController : Controller
    {

        DbConClass context = new DbConClass();

        //index-> getAll Departments method 
        public IActionResult Index()
        {
            //refrence to dbConClass 

            
            //return the db set values into a list

        List<Department> deptsModel = context.Department.ToList();
            
            //pass the viewName and  the list to the  
            return View("dept",deptsModel);
        }

        //return specific department detail, pass the id and check with FirstOrDefault the first match to this Id-> return it
        public IActionResult Detail(int id) {
            Department depar= context.Department.FirstOrDefault(d=>d.Id==id);

            //pass the view file name and the value we stored the id in
            return View("Detail",depar);
        }

        //to add new department u need 2 action methods
        //-first: return empty form 
        //-second: save form data in database

        public IActionResult AddDept()
        {
            return View();
        }

        //<form method="post">
        [HttpPost]
        //pass the obj from department domain model class to save the received data into the domain model class data(insted of string Name ,string ManagerName)
        public IActionResult SaveDept(Department dept)
        {

           
                if (dept.Name != null)
                {
                    context.Department.Add(dept);
                    context.SaveChanges();
                //it'll return to index action method(show all departments) after saving
                    return RedirectToAction("Index");

                }
                //AddDept file
                return View("AddDept", dept);
                //return RedirectToAction("DEatils", new{ id=dept.Id});
            }
        }
    }
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////	
	
4) each action method create new html page to display it 

//that will display list of all departments in table(index action method)

//new view file #dept.cshtml
<!--it'll show the title of the table-->
@{
    ViewData["Title"] = "Departments";
}

<!--return the list from the controller in a table-->
<!--strong type view meaning we specified the model type-->
@model List<Department>

<table class="table table-bordered table-hover">
    <tr>
        <th>Department Name</th>
       <th>Manager Name</th>
    </tr>
    @foreach (var item in Model)
    {
        <tr>
            <td>@item.Name</td>
            <td>@item.ManagerName</td>


            <td><a href="/Dept/detail/@item.Id">Deatil</a></td>
        </tr>
    }
</table>


//////////////////////

//new view file #Detail.cshtml to show specfic department data in atable

@{
    ViewData["Title"] = "Detail";
}

<h1>Details</h1>
<!--show specific depart detail with the name and managername-->
<table class="table table-bordered table-hover">
        <tr>
            <th>Department Name</th>
            <th>Department Manager Name</th>
        </tr>
    <tr>
    <td>@Model.Name</td>
    <td>@Model.ManagerName</td>
    </tr>
</table>
<!--go back to dept controller/index action-->
<a href="/Dept/index">Back To All Departments</a>


//////////////////////////

//new view file #AddDept(AddDept action method)

@{
    ViewData["Title"] = "Add New Department";
}

<!--action-> go to dept controller->savedept action method-->

<form method="post" action="/Dept/SaveDept">

    <div class="form-group">

        <label for="Name"> Name:</label>
        <!--this name attr have to match whats in the domain model and controller property-->
        <input type="text" class="form-control" name="Name" >
        </div>
    <div class="form-group">
        <label for="Manager Name">Manager Name:</label>
        <input type="text" class="form-control" name="ManagerName">
    </div>
  
    <input type="submit" class="btn btn-default">Add Department</>
</form>

