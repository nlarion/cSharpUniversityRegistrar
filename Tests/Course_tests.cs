using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversityRegistrar
{
  public class CourseTest : IDisposable
  {
    public CourseTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=universityregistrar_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void test_coursesEmptyAtFirst()
    {
      int result = Course.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      Course firstCourse = new Course("Underwater Basket Weaving","UBW201");
      Course secondCourse = new Course("Underwater Basket Weaving","UBW201");

      Assert.Equal(firstCourse, secondCourse);
    }

    [Fact]
    public void Test_Save_SavesCourseToDatabase()
    {
      Course testCourse = new Course("Underwater Basket Weaving","UBW201");
      testCourse.Save();

      List<Course> result = Course.GetAll();
      List<Course> testList = new List<Course>{testCourse};
      
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCourseObject()
    {
      Course testCourse = new Course("Underwater Basket Weaving","UBW201");
      testCourse.Save();

      Course savedCourse = Course.GetAll()[0];

      int result = savedCourse.GetId();
      int testId = testCourse.GetId();

      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsCourseInDatabase()
    {
      //Arrange
      Course testCourse = new Course("Underwater Basket Weaving","UBW201");
      testCourse.Save();

      //Act
      Course foundCourse = Course.Find(testCourse.GetId());

      //Assert
      Assert.Equal(testCourse, foundCourse);
    }

    [Fact]
    public void Test_GetStudents_RetrievesAllStudentsWithCourse()
    {
      Course testCourse = new Course("Underwater Basket Weaving","UBW201");
      testCourse.Save();

      Student firstStudent = new Student("James", new DateTime(2001,2,2));
      firstStudent.Save();
      Student secondStudent = new Student("Jimmy", new DateTime(2001,2,2));
      secondStudent.Save();

      testCourse.AddStudent(firstStudent);
      List<Student> testStudentList = new List<Student> {firstStudent};
      List<Student> resultStudentList = testCourse.GetStudents();

      Assert.Equal(testStudentList, resultStudentList);
    }


    [Fact]
    public void Test_AddStudent_AddsStudentToCourse()
    {
      //Arrange
      Course testCourse = new Course("Underwater Basket Weaving","UBW201");
      testCourse.Save();

      Student testStudent = new Student("James", new DateTime(2001,2,2));
      testStudent.Save();

      Student testStudent2 = new Student("Water the garden", new DateTime(2001,2,2));
      testStudent2.Save();

      //Act
      testCourse.AddStudent(testStudent);
      testCourse.AddStudent(testStudent2);

      List<Student> result = testCourse.GetStudents();
      List<Student> testList = new List<Student>{testStudent, testStudent2};

      //Assert
      Assert.Equal(testList, result);
}

    [Fact]
     public void Test_Delete_DeletesCourseFromDatabase()
     {
       //Arrange
       Course testCourse1 = new Course("Home stuff","UBW201");
       testCourse1.Save();

       Course testCourse2 = new Course("Work stuff","UBW201");
       testCourse2.Save();

       //Act
       testCourse1.Delete();
       List<Course> resultcourses = Course.GetAll();
       List<Course> testCourseList = new List<Course> {testCourse2};

       //Assert
       Assert.Equal(testCourseList, resultcourses);
     }

    [Fact]
    public void Test_Delete_DeletesCourseAssociationsFromDatabase()
    {
      Student testStudent = new Student("James", new DateTime(2001,2,2));
      testStudent.Save();

      Course testCourse = new Course("Home stuff","UBW201");
      testCourse.Save();

      testCourse.AddStudent(testStudent);
      testCourse.Delete();

      List<Course> resultStudentcourses = testStudent.GetCourse();
      List<Course> testStudentcourses = new List<Course> {};

      Assert.Equal(testStudentcourses, resultStudentcourses);
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }
  }
}
