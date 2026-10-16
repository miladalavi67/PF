# vendor — offline stand-ins for internal IKCO npm packages

The internal registry (`http://packages.secservice.ikco.com/repository/Npm-Group/`) is not reachable from the
development machine this frontend was built on, so the two `@ikco/*` packages are resolved locally:

| Package | Local source | On the IKCO network |
|---|---|---|
| `@ikco/mfe-core` | `ikco-mfe-core-2.1.12.tgz` (official package, from PTRWorkspace) | `"@ikco/mfe-core": "2.1.12"` |
| `@ikco/frontend-kit` | `ikco-frontend-kit/` — source copy of the kit (IkcoTemplate) taken from the Logestic MFE, exposing the same names/props as kit 1.2.x | `"@ikco/frontend-kit": "^1.2.9"` |

Switching to the registry: add `registry=http://packages.secservice.ikco.com/repository/Npm-Group/` to
`%USERPROFILE%\.npmrc`, change the two entries in `package.json` to the versions above, delete `node_modules` and
`package-lock.json`, run `npm install` and `npm run build`. Pages import only `@ikco/frontend-kit` / `@ikco/mfe-core`,
never this folder, so no source change is needed. If the official kit differs in a prop, `tsc -b` reports it.
