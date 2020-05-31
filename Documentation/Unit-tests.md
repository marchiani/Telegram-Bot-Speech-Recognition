### Unit Tests

Для проведения Unit-тестов будем использовать **Xunit**

        using Xunit;

Для начала нужно создать класс **HomeControllerTests**

      public class HomeControllerTests
      {

      }

Следующим шагом будет добавление методов тестов, например **IndexViewResultNotNull**

        public void IndexViewResultNotNull()
        {
            // Arrange
            HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.NotNull(result);
        }

Метод **IndexViewResultNotNull** тестирует результат метода - возвращаемый объект ViewResult не должен иметь значение null.

Тесты в xUnit определяются в виде методов, каждый метод содержит три логических части - **Arrange**, **Act** и **Assert**.

Модель тестов **Arrange-Act-Assert**:
* **Arrange**: устанавливает начальные условия для выполнения теста
* **Act**: выполняет тест
* **Assert**: верифицирует результат теста

Для проверки результата в классе Assert определено ряд методов:

* **All(collection, action)**: метод подтверждает, что все элементы коллекции collection соответствуют действию action

* **Contains(expectedSubString, actualString)**: метод подтверждает, что строка actualString содержит expectedSubString

* **DoesNotContain(expectedSubString, actualString)**: метод подтверждает, что строка actualString не содержит строку expectedSubString

* **DoesNotMatch(expectedRegexPattern, actualString)**: метод подтверждает, что строка actualString не соответствует регулярному выражению expectedRegexPattern

* **Matches(expectedRegexPattern, actualString)**: метод подтверждает, что строка actualString соответствует регулярному выражению expectedRegexPattern

* **Equal(expected, result)**: метод сравнивает результат теста в виде значения result и ожидаемое значение expected и подтверждает их равенство

* **NotEqual(expected, result)**: метод сравнивает результат теста в виде значения result и ожидаемое значение expected и подтверждает их неравенство

* **Empty(collection)**: метод подтверждает, что коллекция collection пустая

* **NotEmpty(collection)**: метод подтверждает, что коллекция collection не пустая

* **True(result)**: метод подтверждает, что результат теста равен true

* **False(result)**: метод подтверждает, что результат теста равен false

* **IsType(expected, result)**: метод подтверждает, что результат теста имеет тип expected

* **IsNotType(expected, result)**: метод подтверждает, что результат теста не представляет тип expected

* **IsNull(result)**: метод подтверждает, что результат теста имеет значение null

* **IsNotNull(result)**: метод подтверждает, что результат теста не равен null

* **InRange(result, low, high)**: метод подтверждает, что результат теста находится в диапазоне между low и high

* **NotInRange(result, low, high)**: метод подтверждает, что результат теста не принадлежит диапазону от low до high

* **Same(expected, actual)**: метод подтверждает, что ссылки expected и actual указывают на один и тот же объект в памяти

* **NotSame(expected, actual)**: метод подтверждает, что ссылки expected и actual указывают на разные объекты в памяти

* **Throws(exception, expression)**: метод подтверждает, что выражение expression генерирует исключение exception

Тесты можно запустить через меню **Test** -> **Run All Tests**

После этого откроется окно **Test Explorer** или **обозреватель тестов**. Если все нормально, то обозреватель тестов сигнализирует нам зеленым цветом, что все тесты успешно пройдены. Если же нет, то не пройденные тесты будут отмечены красным цветом.
