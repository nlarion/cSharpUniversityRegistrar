using Nancy;
using UniversityRegistrar;
using System.Collections.Generic;
using System;

namespace UniversityRegistrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] =_=>
      {
        List<Course> AllCourses = Course.GetAll();
        return  View ["index.cshtml", AllCourses];
      };
      Get["/students"] = _ =>
      {
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
      Get["/courses"] = _ =>
      {
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };
      Get["/courses/new"] = _ =>
      {
        return View["course_form.cshtml"];
      };
      Post["/courses/new"] = _ =>
      {
        Course newCourse = new Course(Request.Form["course-name"],Request.Form["course-number"]);
        newCourse.Save();
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml",AllCourses];
      };
      Get["/students/new"] = _ =>
      {
        List<Course> AllCourse = Course.GetAll();
        return View["student_form.cshtml", AllCourse];
      };
      Post["/students/new"] = _ =>
      {
        Student newStudent = new Student(Request.Form["student-name"],Request.Form["student-date"]);
        newStudent.Save();
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
      Post["/students/delete"] = _ =>
      {
        Student.DeleteAll();
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
      Get["/students/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Student selectedStudent = Student.Find(parameters.id);
        List<Course> studentCourses = selectedStudent.GetCourse();
        List<Course> AllCourses = Course.GetAll();
        model.Add("student", selectedStudent);
        model.Add("studentCourses", studentCourses);
        model.Add("AllCourses", AllCourses);
        return View["student.cshtml", model];
      };
      Get["/courses/{id}"] = parameters =>
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Course SelectedCourse = Course.Find(parameters.id);
        List<Student> CourseStudent = SelectedCourse.GetStudents();
        List<Student> AllStudents = Student.GetAll();
        model.Add("course", SelectedCourse);
        model.Add("CourseStudent", CourseStudent);
        model.Add("AllStudents", AllStudents);
        return View["course.cshtml", model];
      };
      Post["/courses/delete"] = _ =>
      {
        Course.DeleteAll();
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };
      Post["/student/add_course"] = _ =>
      {
        Course course = Course.Find(Request.Form["course-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        student.AddCourse(course);
        List<Student> AllStudents = Student.GetAll();
        return View["students.cshtml", AllStudents];
      };
      Post["/course/add_student"] = _ =>
      {
        Course course = Course.Find(Request.Form["course-id"]);
        Student student = Student.Find(Request.Form["student-id"]);
        course.AddStudent(student);
        List<Course> AllCourses = Course.GetAll();
        return View["courses.cshtml", AllCourses];
      };
    }
  }
}
