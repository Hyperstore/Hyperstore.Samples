// Sample Domain model definition
// Use Hyperstore Domain Language Editor extension to edit this file.
domain $assemblyname$.MyModel
{
    def entity NamedElement {
        Name : string check error "Name is required"  {return !String.IsNullOrEmpty(self.Name);};
    } 	 

    def entity Library extends NamedElement
    {
        Books   => Book*   : LibraryHasBooks;			
        Members => Member* : LibraryHasMembers;
        Loans   => Loan*   : LibraryHasLoans;
    }

    def entity Book {
        Title  : string;
        Copies : int;
    }

    def entity Member extends NamedElement {
        Library *<= Library : LibraryHasMembers;
    }

    def entity Loan {
        Book   -> Book   : LoanReferencesBook; 
        Member -> Member : LoanReferencesMember;
    }
}