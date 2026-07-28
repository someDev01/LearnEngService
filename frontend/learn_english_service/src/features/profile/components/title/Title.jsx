import styles from '../title/title.module.css';

function Title({title}){
    return(
        <h3 className={styles.title}>
            {title} 
        </h3>
    )
}

export default Title;