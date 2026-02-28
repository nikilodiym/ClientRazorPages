# Імпортуємо модуль os для роботи з файловою системою
import os

# Отримуємо абсолютний шлях до папки, де знаходиться цей Python-скрипт
base_directory = os.getcwd()

# Визначаємо ім'я вихідного файлу, куди будемо дописувати текст
output_file_name = "all_texts.txt"
this_file_name = "get_text.py"

# Формуємо повний шлях до вихідного файлу
output_file_path = os.path.join(base_directory, output_file_name)
this_file_path = os.path.join(base_directory, this_file_name)

# Відкриваємо вихідний файл у режимі запису (w — перезаписує файл)
with open(output_file_path, "w", encoding="utf-8") as output_file:

    # Проходимо по всіх папках і файлах рекурсивно
    for root, dirs, files in os.walk(base_directory):

        # Перебираємо всі файли у поточній папці
        for file_name in files:

            # Формуємо повний шлях до поточного файлу
            file_path = os.path.join(root, file_name)

            # Пропускаємо сам вихідний файл, щоб не читати його повторно
            if file_path == output_file_path or file_path == this_file_path:
                continue

            try:
                # Відкриваємо файл у режимі читання як текст
                with open(file_path, "r", encoding="utf-8") as input_file:

                    # Записуємо заголовок з шляхом до файлу
                    output_file.write(f"\n===== FILE: {file_path} =====\n")

                    # Читаємо весь текст файлу
                    file_content = input_file.read()

                    # Записуємо текст у загальний файл
                    output_file.write(file_content)

                    # Додаємо перенос рядка після файлу
                    output_file.write("\n")

            except Exception as error:
                # Якщо файл неможливо прочитати як текст — фіксуємо помилку
                output_file.write(f"\n===== FILE: {file_path} =====\n")
                output_file.write(f"Не вдалося прочитати файл: {error}\n")
