import json
import os
import tkinter as tk
from tkinter import ttk, messagebox, filedialog
from datetime import datetime
import webbrowser


class LearningTracker:
    def __init__(self):
        self.filename = "learning_progress.json"
        self.data = self.load_data()

    def load_data(self):
        if os.path.exists(self.filename):
            try:
                with open(self.filename, 'r', encoding='utf-8') as f:
                    data = json.load(f)
                    # Проверяем и добавляем отсутствующие ключи
                    return self.fix_missing_data(data)
            except:
                return self.get_default_data()
        return self.get_default_data()

    def fix_missing_data(self, data):
        """Добавляет отсутствующие ключи в существующие данные"""
        default_data = self.get_default_data()

        # Проверяем каждый этап
        for stage_id in default_data['stages']:
            if stage_id in data['stages']:
                # Добавляем resources если их нет
                if 'resources' not in data['stages'][stage_id]:
                    data['stages'][stage_id]['resources'] = default_data['stages'][stage_id]['resources']

                # Обновляем задачи если нужно
                current_tasks = data['stages'][stage_id]['tasks']
                default_tasks = default_data['stages'][stage_id]['tasks']

                # Добавляем новые задачи если их нет
                for task_id in default_tasks:
                    if task_id not in current_tasks:
                        current_tasks[task_id] = default_tasks[task_id]

            else:
                # Добавляем полностью отсутствующий этап
                data['stages'][stage_id] = default_data['stages'][stage_id]

        return data

    def get_default_data(self):
        return {
            "stages": {
                "stage1": {
                    "name": "Основы Python (3-8 недель)",
                    "completed": False,
                    "progress": 0,
                    "resources": {
                        "resource1": {
                            "name": "Программирование на Python (Stepik)",
                            "url": "https://stepik.org/course/67/promo",
                            "completed": False
                        },
                        "resource2": {
                            "name": "Интерактивный тренажер Hexlet",
                            "url": "https://ru.hexlet.io/courses/python-basics",
                            "completed": False
                        },
                        "resource3": {
                            "name": "Python для начинающих (YouTube-канал Егорова)",
                            "url": "https://www.youtube.com/playlist?list=PLQAt0m1f9OHvGM7Y7jZBZS-9wTksAidIF",
                            "completed": False
                        },
                        "resource4": {
                            "name": 'Книга "Изучаем Python" (Эрик Мэтиз)',
                            "url": "https://www.ozon.ru/product/izuchaem-python-programmirovanie-igr-vizualizatsiya-dannyh-prilozheniya-erik-metiz-207759704/",
                            "completed": False
                        }
                    },
                    "tasks": {
                        "task1": {"name": "Зарегистрироваться на Coding Game", "completed": False},
                        "task2": {"name": "Решить 20+ простых задач", "completed": False},
                        "task3": {"name": "Создать калькулятор", "completed": False},
                        "task4": {"name": "Создать конвертер валют", "completed": False},
                        "task5": {"name": "Создать программу для учета финансов", "completed": False}
                    }
                },
                "stage2": {
                    "name": "Освоение Git и GitHub (1-2 недели)",
                    "completed": False,
                    "progress": 0,
                    "resources": {
                        "resource1": {
                            "name": "GitHub Skills",
                            "url": "https://skills.github.com/",
                            "completed": False
                        },
                        "resource2": {
                            "name": "Learn Git Branching",
                            "url": "https://learngitbranching.js.org/",
                            "completed": False
                        },
                        "resource3": {
                            "name": "Pro Git (книга)",
                            "url": "https://git-scm.com/book/ru/v2",
                            "completed": False
                        }
                    },
                    "tasks": {
                        "task1": {"name": "Установить Git на компьютер", "completed": False},
                        "task2": {"name": "Создать аккаунт на GitHub", "completed": False},
                        "task3": {"name": "Создать первый репозиторий", "completed": False},
                        "task4": {"name": "Сделать 5+ коммитов", "completed": False},
                        "task5": {"name": "Создать ветку и merge request", "completed": False}
                    }
                },
                "stage3": {
                    "name": "Углубленное изучение Python (1-2 месяца)",
                    "completed": False,
                    "progress": 0,
                    "resources": {
                        "resource1": {
                            "name": "Python. К вершинам мастерства (Лучано Рамальо)",
                            "url": "https://www.ozon.ru/product/python-k-vershinam-masterstva-144901049/",
                            "completed": False
                        },
                        "resource2": {
                            "name": "Официальная документация Python",
                            "url": "https://docs.python.org/3/tutorial/index.html",
                            "completed": False
                        }
                    },
                    "tasks": {
                        "task1": {"name": "Изучить ООП: классы и объекты", "completed": False},
                        "task2": {"name": "Изучить наследование и полиморфизм", "completed": False},
                        "task3": {"name": "Освоить виртуальные окружения", "completed": False},
                        "task4": {"name": "Изучить менеджер пакетов pip", "completed": False},
                        "task5": {"name": "Создать телефонный справочник", "completed": False},
                        "task6": {"name": "Создать игру 'Виселица'", "completed": False},
                        "task7": {"name": "Создать парсер сайта", "completed": False},
                        "task8": {"name": "Создать Telegram-бота", "completed": False}
                    }
                },
                "stage4": {
                    "name": "Выбор специализации (2-4 месяца)",
                    "completed": False,
                    "progress": 0,
                    "resources": {
                        "web_resource1": {
                            "name": "Официальная документация Django",
                            "url": "https://docs.djangoproject.com/",
                            "completed": False
                        },
                        "web_resource2": {
                            "name": "Официальная документация Flask",
                            "url": "https://flask.palletsprojects.com/",
                            "completed": False
                        },
                        "web_resource3": {
                            "name": "Web-разработка на Python (Stepik)",
                            "url": "https://stepik.org/course/154538/promo",
                            "completed": False
                        },
                        "ds_resource1": {
                            "name": "Machine Learning на Coursera",
                            "url": "https://www.coursera.org/specializations/machine-learning-introduction",
                            "completed": False
                        }
                    },
                    "tasks": {
                        "task1": {"name": "Выбрать специализацию", "completed": False},
                        "task2": {"name": "Изучить выбранный фреймворк", "completed": False},
                        "task3": {"name": "Изучить базы данных", "completed": False},
                        "task4": {"name": "Создать проект на выбранном фреймворке", "completed": False},
                        "task5": {"name": "Разработать REST API", "completed": False},
                        "task6": {"name": "Изучить дополнительные библиотеки", "completed": False}
                    }
                }
            },
            "metadata": {
                "start_date": datetime.now().strftime("%Y-%m-%d"),
                "last_update": datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
                "total_progress": 0
            }
        }

    def save_data(self):
        self.data['metadata']['last_update'] = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        with open(self.filename, 'w', encoding='utf-8') as f:
            json.dump(self.data, f, ensure_ascii=False, indent=2)

    def update_task(self, stage_id, task_id, completed):
        self.data['stages'][stage_id]['tasks'][task_id]['completed'] = completed
        self.update_stage_progress(stage_id)
        self.update_total_progress()
        self.save_data()

    def update_resource(self, stage_id, resource_id, completed):
        self.data['stages'][stage_id]['resources'][resource_id]['completed'] = completed
        self.update_stage_progress(stage_id)
        self.update_total_progress()
        self.save_data()

    def update_stage_progress(self, stage_id):
        stage = self.data['stages'][stage_id]
        tasks = stage['tasks']
        resources = stage.get('resources', {})

        completed_tasks = sum(1 for task in tasks.values() if task['completed'])
        completed_resources = sum(1 for resource in resources.values() if resource['completed'])

        total_items = len(tasks) + len(resources)
        progress = ((completed_tasks + completed_resources) / total_items) * 100 if total_items > 0 else 0

        stage['progress'] = progress
        stage['completed'] = progress >= 100

    def update_total_progress(self):
        stages = self.data['stages']
        total_progress = sum(stage['progress'] for stage in stages.values()) / len(stages)
        self.data['metadata']['total_progress'] = total_progress

    def export_progress(self, filename):
        with open(filename, 'w', encoding='utf-8') as f:
            json.dump(self.data, f, ensure_ascii=False, indent=2)

    def import_progress(self, filename):
        try:
            with open(filename, 'r', encoding='utf-8') as f:
                data = json.load(f)
                self.data = self.fix_missing_data(data)
            return True
        except:
            return False


class LearningTrackerGUI:
    def __init__(self, root):
        self.root = root
        self.root.title("Методичка: Путь от нуля до Python-разработчика")
        self.root.geometry("1000x800")
        self.tracker = LearningTracker()

        self.setup_ui()
        self.load_progress()

    def setup_ui(self):
        # Main frame
        main_frame = ttk.Frame(self.root, padding="10")
        main_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))

        # Title
        title_label = ttk.Label(main_frame, text="Методичка: Путь от нуля до Python-разработчика",
                                font=("Arial", 16, "bold"))
        title_label.grid(row=0, column=0, columnspan=3, pady=10)

        # Progress bar
        self.progress_var = tk.DoubleVar()
        ttk.Label(main_frame, text="Общий прогресс:", font=("Arial", 12)).grid(row=1, column=0, sticky=tk.W, pady=5)
        self.progress_bar = ttk.Progressbar(main_frame, variable=self.progress_var, maximum=100)
        self.progress_bar.grid(row=1, column=1, sticky=(tk.W, tk.E), pady=5, padx=5)
        self.progress_label = ttk.Label(main_frame, text="0%", font=("Arial", 12))
        self.progress_label.grid(row=1, column=2, padx=5)

        # Notebook for stages
        self.notebook = ttk.Notebook(main_frame)
        self.notebook.grid(row=2, column=0, columnspan=3, sticky=(tk.W, tk.E, tk.N, tk.S), pady=10)

        # Create tabs for each stage
        self.stage_frames = {}
        for stage_id, stage_data in self.tracker.data['stages'].items():
            frame = ttk.Frame(self.notebook, padding="10")
            self.notebook.add(frame, text=stage_data['name'])
            self.stage_frames[stage_id] = frame

        # Buttons
        button_frame = ttk.Frame(main_frame)
        button_frame.grid(row=3, column=0, columnspan=3, pady=10)

        ttk.Button(button_frame, text="Сохранить прогресс", command=self.tracker.save_data).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="Экспорт прогресса", command=self.export_data).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="Импорт прогресса", command=self.import_data).pack(side=tk.LEFT, padx=5)
        ttk.Button(button_frame, text="Сбросить прогресс", command=self.reset_progress).pack(side=tk.LEFT, padx=5)

        # Configure grid weights
        self.root.columnconfigure(0, weight=1)
        self.root.rowconfigure(0, weight=1)
        main_frame.columnconfigure(1, weight=1)
        main_frame.rowconfigure(2, weight=1)

    def create_clickable_link(self, parent, text, url):
        """Создает кликабельную ссылку"""
        link = ttk.Label(parent, text=text, foreground="blue", cursor="hand2")
        link.bind("<Button-1>", lambda e: webbrowser.open_new(url))
        return link

    def load_progress(self):
        # Update overall progress
        total_progress = self.tracker.data['metadata']['total_progress']
        self.progress_var.set(total_progress)
        self.progress_label.config(text=f"{total_progress:.1f}%")

        # Load tasks for each stage
        for stage_id, stage_data in self.tracker.data['stages'].items():
            frame = self.stage_frames[stage_id]

            # Clear existing widgets
            for widget in frame.winfo_children():
                widget.destroy()

            # Create a canvas and scrollbar for scrollable content
            canvas = tk.Canvas(frame)
            scrollbar = ttk.Scrollbar(frame, orient="vertical", command=canvas.yview)
            scrollable_frame = ttk.Frame(canvas)

            scrollable_frame.bind(
                "<Configure>",
                lambda e: canvas.configure(scrollregion=canvas.bbox("all"))
            )

            canvas.create_window((0, 0), window=scrollable_frame, anchor="nw")
            canvas.configure(yscrollcommand=scrollbar.set)

            # Stage progress
            stage_progress_var = tk.DoubleVar(value=stage_data['progress'])
            stage_progress = ttk.Progressbar(scrollable_frame, variable=stage_progress_var, maximum=100)
            stage_progress.pack(fill=tk.X, pady=5)
            ttk.Label(scrollable_frame, text=f"Прогресс этапа: {stage_data['progress']:.1f}%",
                      font=("Arial", 11, "bold")).pack(anchor=tk.W, pady=(0, 15))

            # Resources section (только если есть ресурсы)
            resources = stage_data.get('resources', {})
            if resources:
                ttk.Label(scrollable_frame, text="Ресурсы для изучения:",
                          font=("Arial", 12, "bold")).pack(anchor=tk.W, pady=(10, 5))

                for resource_id, resource_data in resources.items():
                    resource_frame = ttk.Frame(scrollable_frame)
                    resource_frame.pack(fill=tk.X, pady=2)

                    var = tk.BooleanVar(value=resource_data['completed'])
                    cb = ttk.Checkbutton(
                        resource_frame,
                        variable=var,
                        command=lambda s=stage_id, r=resource_id, v=var: self.on_resource_toggle(s, r, v)
                    )
                    cb.pack(side=tk.LEFT)

                    link = self.create_clickable_link(
                        resource_frame,
                        resource_data['name'],
                        resource_data['url']
                    )
                    link.pack(side=tk.LEFT, padx=5)

            # Tasks section
            ttk.Label(scrollable_frame, text="Практические задания:",
                      font=("Arial", 12, "bold")).pack(anchor=tk.W, pady=(20, 5))

            for task_id, task_data in stage_data['tasks'].items():
                var = tk.BooleanVar(value=task_data['completed'])
                cb = ttk.Checkbutton(
                    scrollable_frame,
                    text=task_data['name'],
                    variable=var,
                    command=lambda s=stage_id, t=task_id, v=var: self.on_task_toggle(s, t, v)
                )
                cb.pack(anchor=tk.W, pady=2)

            # Pack canvas and scrollbar
            canvas.pack(side="left", fill="both", expand=True)
            scrollbar.pack(side="right", fill="y")

    def on_task_toggle(self, stage_id, task_id, var):
        self.tracker.update_task(stage_id, task_id, var.get())
        self.load_progress()

    def on_resource_toggle(self, stage_id, resource_id, var):
        self.tracker.update_resource(stage_id, resource_id, var.get())
        self.load_progress()

    def export_data(self):
        filename = filedialog.asksaveasfilename(
            defaultextension=".json",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")]
        )
        if filename:
            self.tracker.export_progress(filename)
            messagebox.showinfo("Успех", "Прогресс экспортирован успешно!")

    def import_data(self):
        filename = filedialog.askopenfilename(
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")]
        )
        if filename and self.tracker.import_progress(filename):
            self.load_progress()
            messagebox.showinfo("Успех", "Прогресс импортирован успешно!")
        else:
            messagebox.showerror("Ошибка", "Не удалось импортировать прогресс")

    def reset_progress(self):
        if messagebox.askyesno("Подтверждение", "Вы уверены, что хотите сбросить весь прогресс?"):
            self.tracker.data = self.tracker.get_default_data()
            self.tracker.save_data()
            self.load_progress()


def main():
    root = tk.Tk()
    app = LearningTrackerGUI(root)
    root.mainloop()


if __name__ == "__main__":
    main()