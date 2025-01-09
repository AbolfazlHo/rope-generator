
# Rope Generator Unity Package

## Description

The **Rope Generator** is a Unity package designed to simplify the creation of dynamic ropes in Unity projects. This package allows you to easily generate customizable ropes with adjustable width, length, and material in both Edit Mode and Play Mode.

### Key Features

- **Customizable Ropes:** Adjust width, length, and material to fit your project's needs.
- **Editor Integration:** Use the provided editor window to set parameters, generate ropes, and control physics simulations directly within Unity.
- **Dynamic Physics:** Ropes are composed of interconnected "rope segments" using Unity's `CharacterJoint` components, ensuring realistic physics behavior (e.g., rope bending and collision).



## Installation

### Method 1: Import from GitHub (Recommended)

1. Copy the URL of the `package.json`: ```https://github.com/AbolfazlHo/rope-generator/blob/develop/Assets/ir.soor.rope-generator/package.json```

2. Open your Unity project.
3. In Unity Editor, navigate to **Window > Package Manager**.
    ![Package Manager Menu](images/package_manager_menu.png)
4. Click the **+** icon in the top-left corner of the Package Manager and select **"Add package from Git URL"**.
5. Paste the copied URL and click **"Add"**.
    ![Add Package from Git URL](images/add_package_from_git_url.png)

Unity will download and install the package and its dependencies.

**Watch the installation process on YouTube:** [Link to YouTube video]



### Method 2: Import from Unity Asset Store

1. Open your Unity project.
2. Go to [Unity Asset Store](https://assetstore.unity.com/) and search for "Character Joint Based Rope Generator."
    ![Package Page](images/asset_store_page.png)
3. Click **"Add to Cart"**, then select **"Open in Unity"** when prompted.
    ![Open in Unity](images/open_in_unity_popup.png)
4. Unity will open the Package Manager and display the package. Click **"Download and Install"**.

**Watch the Asset Store installation process on YouTube:** [Link to YouTube video]



## Setup Before Use

Before using the package, update Unity's time settings:

1. Navigate to **Edit > Project Settings > Time**.
2. Adjust the parameters as shown:
    ![Time Settings](images/time_settings.png)

**Watch the setup process on YouTube:** [Link to YouTube video]



## How to Use the Package

### In Edit Mode

1. After installation, you'll find a new menu item: **Tools > Rope Editor**.
2. Open the **Rope Editor** window to configure parameters and generate ropes.
    ![Tools Menu](images/tools_menu.png)
    ![Rope Editor Window](images/rope_editor_window.png)

**Watch the usage guide for Edit Mode on YouTube:** [Link to YouTube video]



### In Play Mode

You can dynamically generate ropes in Play Mode by calling the constructor method of the rope class in your scripts.

**Watch the Play Mode usage guide on YouTube:** [Link to YouTube video]



## Contribution

Contributions are welcome!

- Fork the `main` branch, make your changes, and submit a pull request.
- Share your feedback or record videos showcasing your use of the package.



## Special Thanks

Special thanks to the [Smon YouTube Channel](https://www.youtube.com/watch?v=pKSUhsyrj_4), whose tutorial inspired the development of this package.



## License

This project is licensed under the MIT License.

