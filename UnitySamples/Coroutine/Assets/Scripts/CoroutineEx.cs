using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineEx : MonoBehaviour
{
    public class Person
    {
        public string firstName;
        public string lastName;

        public Person(string fName, string lName)
        {
            firstName = fName;
            lastName = lName;
        }
    }

    public class People : IEnumerable
    {
        Person[] _people;

        public People(Person[] pArr)
        {
            _people = new Person[pArr.Length];
            for (int idx = 0; idx < pArr.Length; idx++)
                _people[idx] = pArr[idx];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public PeopleEnum GetEnumerator()
        {
            return new PeopleEnum(_people);
        }
    }

    public class PeopleEnum : IEnumerator
    {
        public Person[] _people;

        int pos = -1;

        public PeopleEnum(Person[] list)
        {
            _people = list;
        }

        public bool MoveNext()
        {
            pos++;
            return (pos < _people.Length);
        }

        public void Reset()
        {
            pos = -1;
        }

        object IEnumerator.Current { get { return Current; } }

        public Person Current { get { return _people[pos]; } }
    }

    private void Start()
    {
        //Person[] peopleArray = new Person[3]
        //{
        //    new Person("John", "Smith"),
        //    new Person("Jim", "Johnson"),
        //    new Person("Sue", "Rabon"),
        //};

        //People peopleList = new People(peopleArray);
        //foreach(Person p in peopleList)
        //{
        //    Debug.Log(p.firstName + " " + p.lastName);
        //}

        StartCoroutine(CoPeopleList());
    }

    IEnumerator CoPeopleList()
    {
        
    }
}
