# 📝 Commit Convention Guide

This project uses a Gitmoji + Conventional Commits hybrid for better readability and context.

---

## 🎯 Format

```
<gitmoji> <type>(<optional-scope>): <short description>

<optional body>  
```

### Examples

``` 
✨ feat(core): add interactive file filter menu  
🐛 fix(regex): correct folder exclusion logic  
🛡️ docs(security): add initial SECURITY.md  
```

---

## 🧩 Gitmoji + Types

| Gitmoji | Type      | Use case                                 |
|--------:|-----------|-------------------------------------------|
| ✨       | feat      | Add a new feature                         |
| 🐛       | fix       | Fix a bug or unexpected behavior          |
| 📚       | docs      | Documentation changes only                |
| 🎨       | style     | Code formatting (no logic change)         |
| ♻️       | refactor  | Code refactoring without feature change  |
| ✅       | test      | Add or update tests                       |
| 🔧       | chore     | Build scripts, configs, maintenance       |
| ⬆️       | build     | Dependency or build tool changes          |
| 🚀       | perf      | Improve performance                       |
| 🛡️       | security  | Security-related additions or fixes       |
| 🔥       | remove    | Remove code or files                      |
| ➕       | add       | Add something non-code or support files   |

---

## ✍️ Tips

- Keep commit messages short and meaningful.
- Use English to maintain international readability.
- Group related changes into a single commit if possible.
- Avoid committing generated files unless needed (e.g., .txt outputs, binaries).
- For more Gitmoji references, visit: [https://gitmoji.dev](https://gitmoji.dev)
