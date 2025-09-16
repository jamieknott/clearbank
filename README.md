### DEV NOTES
Create tests first to ensure logic remains unchanged after refactor.
* Concrete DataStore means I cannot mock the response, decreasing test ability

Concrete DataStore from PaymentService could be removed adding an interface layer.
* Dupe stores, could be one?

PaymentScheme switch refactor to remove the long conditional.
* Factory?
	* Easy to extend

### IMPLEMENTATION
Create DataService to add a layer of abstraction between the PaymentService and AccountDataStore
* DataService will ??

Create Scheme factory to replace the switch section making the PaymentService cleaner and isolates the validation logic.
* Easier to add new schemas and update existing ones.