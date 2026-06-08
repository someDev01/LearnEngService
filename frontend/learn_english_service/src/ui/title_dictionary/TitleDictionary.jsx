import styles from '../title_dictionary/title_dictionary.module.css';

function TitleDictionary({count}){
    return(
        <div className={styles.title}><p>Словарь ({count})</p></div>
    )
}

export default TitleDictionary;