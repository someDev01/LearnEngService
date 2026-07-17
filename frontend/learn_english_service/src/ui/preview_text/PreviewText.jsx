import styles from '../preview_text/preview_text.module.css';

function PreviewText(){
    return(
        <>
            <div className={styles.block_text}>
                <div className={styles.text_preview}>
                    <h5>Смотри видео.</h5>     
                    <h5>Сохраняй заметки.</h5>
                    <h5 style={{color:'#ff7300'}}>Закрепляй слова.</h5>
                </div>
            </div>
            <div className={styles.block_description}>
                <div className={styles.text_description}>
                    <p>Добавляй слова прямо из видео с субтитрами или вручную - все собирается в удобном едином словаре</p>
                </div>
            </div>
        </>
    )
}

export default PreviewText;