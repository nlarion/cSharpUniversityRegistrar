using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversityRegistrar
{
  public class StudentTest : IDisposable
  {
    public StudentTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=universityregistrar_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameName()
    {

      //Arrange, Act
      Student firstStudent = new Student("Dean");
      Student secondStudent = new Student("Dean");

      //Assert
      Assert.Equal(firstStudent, secondStudent);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Student testStudent = new Student("Dean");
      testStudent.Save();

      //Act
      List<Student> result = Student.GetAll();
      List<Student> testList = new List<Student>{testStudent};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Student testStudent = new Student("Dean");
      testStudent.Save();

      //Act
      Student savedStudent = Student.GetAll()[0];

      int result = savedStudent.GetId();
      int testId = testStudent.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsStudentInDatabase()
    {
      //Arrange
      Student testStudent = new Student("Dean");
      testStudent.Save();

      //Act
      Student foundStudent = Student.Find(testStudent.GetId());

      //Assert
      Assert.Equal(testStudent, foundStudent);
    }

    [Fact]
    public void Test_Delete_DeletesStudentAssociationsFromDatabase()
    {
      Course testCourse = new Course("Math");
      testCourse.Save();

      string testName = "Dean";
      Student testStudent = new Student(testName);
      testStudent.Save();

      testStudent.AddCourse(testCourse);
      testStudent.Delete();

      List<Student> resultCourseStudents = testCourse.GetStudents();
      List<Student> testCourseStudents = new List<Student> {};

      Assert.Equal(testCourseStudents, resultCourseStudents);
    }
  }
}
