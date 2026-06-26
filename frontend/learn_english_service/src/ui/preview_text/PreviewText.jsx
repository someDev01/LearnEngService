import styles from '../preview_text/preview_text.module.css';


function PreviewText(){
    return(
        <>
            <div className={styles.block_text}>
                <div className={styles.text_preview}>
                    <p>VoClip - собирай свой английский из видео и реального контекста</p>
                </div>
            </div>
            <div className={styles.block_description}>
                <div className={styles.text_description}>
                    <p>Короткие видео, заметки, личный словарь, тренировка, удобный интерфейс. Все в одном легком веб‑приложении</p>
                </div>
            </div>
        </>
    )
}

export default PreviewText;