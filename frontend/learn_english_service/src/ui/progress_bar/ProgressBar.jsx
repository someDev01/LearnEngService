import styles from '../progress_bar/progress_bar.module.css';

function ProggressBar({currentIndex, total}){
    return(
        <div className={styles.progress_bar}>
            <header className={styles.title}>
                <h3>Тренировка на слух</h3>
                <p>{currentIndex+1}/{total}</p>
            </header>
            <div className={styles.slider_bg}>
                <div className={styles.slider} style={{width: `${((currentIndex + 1) / total) * 100}%`}}>
                </div>
            </div>
        </div>
    )
}
export default ProggressBar;