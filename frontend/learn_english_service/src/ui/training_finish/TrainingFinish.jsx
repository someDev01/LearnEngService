import { BadgeX, BookOpen, CircleCheckBig, RotateCcw } from 'lucide-react';
import ButtonRestart from '../button_restart/ButtonRestart';
import styles from '../training_finish/training_finish.module.css';

function TrainingFinish({currentIndexView, targetLength, wrongTouchCount, onRestartTraining}){
    return(
        <section className={styles.training_finish}>
            <header className={styles.header}>
                <CircleCheckBig size={20} color='#00ff51'/>
                <h3>Тренировки завершена</h3>
            </header>
            <main className={styles.content}>
                <div className={styles.build_result}>
                    <BookOpen size={20} color='#ff6f00'/>
                    <p className={styles.build_text}> Собрано слова <span>{currentIndexView} / {targetLength}</span></p>
                </div>
                <div className={styles.touch_result}>
                    <BadgeX size={20} color='#ff0000'/>
                    <p className={styles.touch_text}> Неверных нажатий <span>{wrongTouchCount}</span></p>
                </div>
                <div className={styles.advice}>
                    <p style={styles.advice_text}>Повторяй слова регулярно — так они быстрее запоминаются</p>
                </div>
            </main>
            <footer>
                <button className={styles.train_again_button} type='button' onClick={onRestartTraining}>
                    Тренироваться снова <RotateCcw size={20} color='black'/>
                </button>
            </footer>
        </section>
    )
}

export default TrainingFinish;