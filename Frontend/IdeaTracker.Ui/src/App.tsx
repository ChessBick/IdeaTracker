import { useState } from "react";
import "./App.css";
import { ideasApi } from "./api/ideasApi";
import { IdeaForm, type IdeaFormValues } from "./components/IdeaForm";
import { IdeaList } from "./components/IdeaList";
import type { IdeaDto } from "./types/idea";

type View = { name: "list" } | { name: "create" } | { name: "edit"; idea: IdeaDto };

function parseTags(tags: string): string[] {
  return tags
    .split(",")
    .map((t) => t.trim())
    .filter(Boolean);
}

function App() {
  const [view, setView] = useState<View>({ name: "list" });
  const [reloadToken, setReloadToken] = useState(0);

  async function handleCreate(values: IdeaFormValues) {
    await ideasApi.create({
      title: values.title,
      description: values.description || undefined,
      tags: parseTags(values.tags),
    });
    setReloadToken((t) => t + 1);
    setView({ name: "list" });
  }

  async function handleUpdate(idea: IdeaDto, values: IdeaFormValues) {
    await ideasApi.update(idea.id, {
      title: values.title,
      description: values.description || undefined,
      status: values.status,
      tags: parseTags(values.tags),
    });
    setReloadToken((t) => t + 1);
    setView({ name: "list" });
  }

  return (
    <div className="app">
      <header className="app-header">
        <div>
          <h1>Idea Tracker</h1>
          <p className="app-subtitle">Capture, refine, and track your best ideas.</p>
        </div>
        {view.name === "list" && (
          <button type="button" className="btn-primary" onClick={() => setView({ name: "create" })}>
            + New idea
          </button>
        )}
      </header>

      <main className="card">
        {view.name === "list" && (
          <IdeaList reloadToken={reloadToken} onEdit={(idea) => setView({ name: "edit", idea })} />
        )}

        {view.name === "create" && (
          <IdeaForm submitLabel="Create idea" onSubmit={handleCreate} onCancel={() => setView({ name: "list" })} />
        )}

        {view.name === "edit" && (
          <IdeaForm
            idea={view.idea}
            submitLabel="Save changes"
            onSubmit={(values) => handleUpdate(view.idea, values)}
            onCancel={() => setView({ name: "list" })}
          />
        )}
      </main>
    </div>
  );
}

export default App;
