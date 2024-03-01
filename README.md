# Be Heroes *(architecture example)*
> [!NOTE]
> **The project was created as part of the training course "Architecture of mobile games".**

![Gameplay](https://github.com/paul-martyn/Be-Heroes/blob/main/ReadmeContent/gameplay.gif?raw=true)

**The main infrastructural sides of the project:**

- **Input** *(wrapper over third-party plugin "SimpleInput")*

- **Control over correct transitions between different game states** *(state machine)*

- **Object lifecycle control** *(factory)*

- **Data saving and loading** *(serialization to json and saving to PlayerPrefs)*

- **Asset management** *(addressable asset system)*

- **Storing static data** *(scriptable objects)*

  

> [!IMPORTANT]
> **For demonstration purposes, the project does not use third-party libraries like Zenject for dependency injection, but uses a standalone solution - a variant of the Service Locator pattern**.

