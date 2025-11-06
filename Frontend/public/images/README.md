# Images Folder Structure

## 📂 Folder Organization

```
images/
├── movies/     # Movie posters (300x450px recommended)
└── series/     # Series posters (300x450px recommended)
```

## 🎨 Image Guidelines

### Recommended Dimensions:

-   **Movie/Series Cards**: 300x450px (2:3 ratio)
-   **Hero/Banner Images**: 1920x800px (wide format)

### File Naming Convention:

-   Use lowercase
-   Replace spaces with dashes
-   Example: `inception.jpg`, `breaking-bad.jpg`

## 💡 Usage in Database

When adding movies/series, set the `imageUrl` field to:

```
/images/movies/inception.jpg
/images/series/breaking-bad.jpg
```

## 📝 Example

If you place an image at:

```
Frontend/public/images/movies/inception.jpg
```

In the database, set:

```json
{
    "title": "Inception",
    "imageUrl": "/images/movies/inception.jpg"
}
```

The frontend will automatically load it from the public folder.
